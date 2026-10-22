using System.Text;

namespace IngameScript
{
    public partial class Program
    {
        public class EventLog
        {
            private struct Entry
            {
                public double Time;
                public MinerState From, To;
                public ReturnReason Reason;
                public string Note;
            }

            private readonly Entry[] _entries;
            private int _head; // index of next write
            private int _count;

            public EventLog(int capacity)
            {
                _entries = new Entry[capacity < 1 ? 1 : capacity];
            }

            public int Count { get { return _count; } }

            // note: an optional constant string (e.g. a Hold reason); may be null
            public void Add(double time, MinerState from, MinerState to, ReturnReason reason, string note)
            {
                _entries[_head].Time = time;
                _entries[_head].From = from;
                _entries[_head].To = to;
                _entries[_head].Reason = reason;
                _entries[_head].Note = note;
                _head = (_head + 1) % _entries.Length;
                if (_count < _entries.Length)
                    _count++;
            }

            // newest first, at most maxLines lines, each ending in '\n'
            public void Render(StringBuilder sb, int maxLines)
            {
                int n = _count < maxLines ? _count : maxLines;
                for (int i = 0; i < n; i++)
                {
                    int idx = (_head - 1 - i + _entries.Length * 2) % _entries.Length;
                    SbFormat.AppendTime(sb, _entries[idx].Time);
                    sb.Append(' ');
                    sb.Append(Names.State[(int)_entries[idx].From]);
                    sb.Append('>');
                    sb.Append(Names.State[(int)_entries[idx].To]);
                    if (_entries[idx].Reason != ReturnReason.None)
                    {
                        sb.Append(' ');
                        sb.Append(Names.Reason[(int)_entries[idx].Reason]);
                    }
                    string note = _entries[idx].Note;
                    if (!string.IsNullOrEmpty(note))
                    {
                        sb.Append(' ');
                        sb.Append(note);
                    }
                    sb.Append('\n');
                }
            }

            public void Clear()
            {
                for (int i = 0; i < _entries.Length; i++)
                    _entries[i].Note = null;
                _head = 0;
                _count = 0;
            }
        }
    }
}
