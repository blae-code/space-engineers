---
name: ask
description: Read-only question about this codebase (where is X, what calls Y, how does Z work)
invokable: true
---

Answer the question about this repository. This is READ-ONLY: never create, edit or delete a file,
and never run a command that changes anything (no build, no git commit, no file writes).

- Find the answer in the code itself. Search for the symbol, then read the code around it.
- Cite every claim as `path:line`. If you did not read a line, do not cite it.
- If the code does not answer the question (game behaviour, Space Engineers API facts, design intent),
  say so plainly: "not in the code — ask Claude Code". Never guess.
- Keep the answer short: the direct answer first, then the citations.
