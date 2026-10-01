using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public enum MenuActionKind : byte { None, Redraw, Command, SetValue, Select }

        public struct MenuAction
        {
            public MenuActionKind Kind;
            public string Command;   // Command: run-argument text, e.g. "START"
            public int Field;        // SetValue: SettingsSchema index
            public double Value;     // SetValue
            public int Index;        // Select: entry index on a list page
        }

        // Where a Setting item reads its value. The drone answers from its own Settings; the console
        // from its copy of the selected drone's config, marking edits it has sent but not seen echoed.
        public interface IMenuValues
        {
            bool TryGet(int field, out double value);   // false = not known yet (rendered "--", not editable)
            bool IsPending(int field);
        }

        // A page whose entries are supplied at render time (the console's drone roster).
        public interface IMenuList
        {
            int Count { get; }
            void AppendEntry(StringBuilder sb, int index);
        }

        public enum MenuItemKind : byte { Page, Setting, Command }

        public class MenuItem
        {
            public readonly string Label;
            public readonly MenuItemKind Kind;
            public readonly MenuPage Target;     // Page
            public readonly int Field;           // Setting
            public readonly string Command;      // Command
            public readonly string Confirm;      // Command: question asked first, or null

            public MenuItem(string label, MenuItemKind kind, MenuPage target, int field, string command, string confirm)
            {
                Label = label; Kind = kind; Target = target; Field = field; Command = command; Confirm = confirm;
            }
        }

        public class MenuPage
        {
            public string Title;                 // mutable: the console retitles the drone page per drone
            public MenuPage Parent;
            public IMenuList List;               // when set, the page shows List entries instead of Items
            public readonly List<MenuItem> Items = new List<MenuItem>();
            public int Cursor, Scroll;

            public MenuPage(string title) { Title = title; }

            public int Count { get { return List != null ? List.Count : Items.Count; } }

            public MenuPage AddPage(string label, MenuPage target)
            {
                target.Parent = this;
                Items.Add(new MenuItem(label, MenuItemKind.Page, target, -1, null, null));
                return this;
            }

            public MenuPage AddSetting(int field)
            {
                Items.Add(new MenuItem(SettingsSchema.Fields[field].Label, MenuItemKind.Setting, null, field, null, null));
                return this;
            }

            public MenuPage AddCommand(string label, string command, string confirm = null)
            {
                Items.Add(new MenuItem(label, MenuItemKind.Command, null, -1, command, confirm));
                return this;
            }

            // One sub-page per settings group (Job, Behaviour, Flight, Energy), each with its fields.
            public MenuPage AddSettingsPages()
            {
                for (int g = 0; g < SettingsSchema.Groups.Length; g++)
                {
                    var page = new MenuPage(SettingsSchema.Groups[g]);
                    for (int i = 0; i < SettingsSchema.Fields.Length; i++)
                        if (SettingsSchema.Fields[i].Group == SettingsSchema.Groups[g]) page.AddSetting(i);
                    AddPage(SettingsSchema.Groups[g] + " settings", page);
                }
                return this;
            }
        }

        // PAM-style menu driven by UP / DOWN / APPLY / BACK, enhanced: typed fields, accelerating number
        // steps, confirmation for destructive commands, scrolling for small screens, breadcrumb title.
        // Handle and Render do not allocate.
        public class Menu
        {
            public const int LabelWidth = 15;
            public MenuPage Root { get; private set; }
            public MenuPage Current { get; private set; }
            public IMenuValues Values;
            public bool Editing { get; private set; }
            public bool Confirming { get; private set; }
            public double EditValue { get; private set; }
            public int StepMultiplier { get; private set; }
            int _runDir, _runLen;

            public Menu(MenuPage root, IMenuValues values)
            {
                Root = root; Current = root; Values = values; StepMultiplier = 1;
            }

            // Jump to a page (e.g. the console opening a drone from the roster). Cancels any edit.
            public void Open(MenuPage page)
            {
                Current = page; page.Cursor = 0; page.Scroll = 0;
                Editing = false; Confirming = false;
            }

            MenuItem Selected
            {
                get
                {
                    var p = Current;
                    return p.List == null && p.Cursor >= 0 && p.Cursor < p.Items.Count ? p.Items[p.Cursor] : null;
                }
            }

            public MenuAction Handle(Cmd c)
            {
                var a = new MenuAction();
                if (Confirming)
                {
                    Confirming = false;
                    if (c == Cmd.Apply) { a.Kind = MenuActionKind.Command; a.Command = Selected.Command; return a; }
                    a.Kind = MenuActionKind.Redraw;
                    return a;
                }
                if (Editing) return HandleEdit(c);

                var p = Current;
                switch (c)
                {
                    case Cmd.Up:
                    case Cmd.Down:
                        int n = p.Count;
                        if (n == 0) return a;
                        p.Cursor = (p.Cursor + (c == Cmd.Up ? n - 1 : 1)) % n;
                        a.Kind = MenuActionKind.Redraw;
                        return a;
                    case Cmd.Back:
                        if (p.Parent == null) return a;
                        Current = p.Parent;
                        a.Kind = MenuActionKind.Redraw;
                        return a;
                    case Cmd.Apply:
                        if (p.List != null)
                        {
                            if (p.Cursor >= p.List.Count) return a;
                            a.Kind = MenuActionKind.Select; a.Index = p.Cursor;
                            return a;
                        }
                        var item = Selected;
                        if (item == null) return a;
                        if (item.Kind == MenuItemKind.Page)
                        {
                            Open(item.Target);
                            a.Kind = MenuActionKind.Redraw;
                            return a;
                        }
                        if (item.Kind == MenuItemKind.Command)
                        {
                            if (item.Confirm != null) { Confirming = true; a.Kind = MenuActionKind.Redraw; return a; }
                            a.Kind = MenuActionKind.Command; a.Command = item.Command;
                            return a;
                        }
                        double v;
                        if (Values == null || !Values.TryGet(item.Field, out v)) return a;
                        if (SettingsSchema.Fields[item.Field].Kind == FieldKind.Toggle)
                        {
                            a.Kind = MenuActionKind.SetValue; a.Field = item.Field; a.Value = v > 0.5 ? 0 : 1;
                            return a;
                        }
                        Editing = true; EditValue = v; _runDir = 0; _runLen = 0; StepMultiplier = 1;
                        a.Kind = MenuActionKind.Redraw;
                        return a;
                }
                return a;
            }

            MenuAction HandleEdit(Cmd c)
            {
                var a = new MenuAction();
                var item = Selected;
                var f = SettingsSchema.Fields[item.Field];
                if (c == Cmd.Back) { Editing = false; a.Kind = MenuActionKind.Redraw; return a; }
                if (c == Cmd.Apply)
                {
                    Editing = false;
                    a.Kind = MenuActionKind.SetValue; a.Field = item.Field; a.Value = EditValue;
                    return a;
                }
                if (c != Cmd.Up && c != Cmd.Down) return a;
                int dir = c == Cmd.Up ? 1 : -1;
                if (f.Kind == FieldKind.Choice)
                {
                    int count = f.Choices.Length;
                    EditValue = ((int)EditValue + dir + count) % count;
                }
                else
                {
                    // Repeated presses in one direction speed up: x1 for 3 presses, x5 for 3, then x10.
                    if (dir == _runDir) _runLen++; else { _runDir = dir; _runLen = 1; }
                    StepMultiplier = _runLen <= 3 ? 1 : _runLen <= 6 ? 5 : 10;
                    double v = EditValue + dir * f.Step * StepMultiplier;
                    v = Math.Max(f.Min, Math.Min(f.Max, v));
                    EditValue = f.Kind == FieldKind.Int ? Math.Round(v) : Math.Round(v, Math.Max(f.Decimals, 2));
                }
                a.Kind = MenuActionKind.Redraw;
                return a;
            }

            // Breadcrumb title plus at most maxLines - 1 item lines. Every line ends with '\n'.
            public void Render(StringBuilder sb, int maxLines)
            {
                AppendCrumb(sb, Current);
                sb.Append('\n');
                var p = Current;
                if (Confirming)
                {
                    sb.Append("  ").Append(Selected.Confirm).Append('\n');
                    sb.Append("  APPLY = yes   BACK = no\n");
                    return;
                }
                int n = p.Count, window = Math.Max(1, maxLines - 1);
                if (n == 0) { sb.Append("  (none)\n"); return; }
                if (p.Cursor >= n) p.Cursor = n - 1;
                bool scrolling = n > window;
                if (scrolling) window = Math.Max(1, window - 2);   // two lines for the more-markers
                if (p.Cursor < p.Scroll) p.Scroll = p.Cursor;
                if (p.Cursor >= p.Scroll + window) p.Scroll = p.Cursor - window + 1;
                if (p.Scroll > n - window) p.Scroll = Math.Max(0, n - window);
                int end = Math.Min(n, p.Scroll + window);
                if (scrolling)
                {
                    if (p.Scroll > 0) SbFormat.AppendInt(sb.Append("  ^ "), p.Scroll).Append(" more\n");
                    else sb.Append('\n');
                }
                for (int i = p.Scroll; i < end; i++)
                {
                    sb.Append(i == p.Cursor ? "> " : "  ");
                    if (p.List != null) p.List.AppendEntry(sb, i);
                    else AppendItem(sb, p.Items[i], i == p.Cursor);
                    sb.Append('\n');
                }
                if (scrolling)
                {
                    if (end < n) SbFormat.AppendInt(sb.Append("  v "), n - end).Append(" more\n");
                    else sb.Append('\n');
                }
            }

            void AppendItem(StringBuilder sb, MenuItem item, bool selected)
            {
                if (item.Kind == MenuItemKind.Page) { sb.Append(item.Label).Append(" >"); return; }
                if (item.Kind == MenuItemKind.Command) { sb.Append(item.Label); return; }
                sb.Append(item.Label);
                for (int k = item.Label.Length; k < LabelWidth; k++) sb.Append(' ');
                sb.Append(' ');
                if (selected && Editing)
                {
                    SettingsSchema.AppendValue(sb.Append('['), item.Field, EditValue).Append(']');
                    if (StepMultiplier > 1) SbFormat.AppendInt(sb.Append(" x"), StepMultiplier);
                    return;
                }
                double v;
                if (Values == null || !Values.TryGet(item.Field, out v)) { sb.Append("--"); return; }
                SettingsSchema.AppendValue(sb, item.Field, v);
                if (Values.IsPending(item.Field)) sb.Append(" *");
            }

            static void AppendCrumb(StringBuilder sb, MenuPage p)
            {
                if (p.Parent != null) { AppendCrumb(sb, p.Parent); sb.Append(" > "); }
                sb.Append(p.Title);
            }
        }

        // IMenuValues over a live Settings object (the drone's own menu).
        public class SettingsValues : IMenuValues
        {
            public Settings Settings;
            public SettingsValues(Settings s) { Settings = s; }
            public bool TryGet(int field, out double value)
            {
                value = SettingsSchema.Get(Settings, field);
                return field >= 0 && field < SettingsSchema.Fields.Length;
            }
            public bool IsPending(int field) { return false; }
        }
    }
}
