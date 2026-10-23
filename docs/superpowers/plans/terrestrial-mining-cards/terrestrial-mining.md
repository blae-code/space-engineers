# Terrestrial Mining Script Cards

Cards are sequenced after the archived C01-C32 fleet work. The cards in this deck are reviewed before
execution and should each land as a separate, scoped change.

| Card | Summary | Depends on | Executor |
| --- | --- | --- | --- |
| C33 | TMS handover codec |  | local |
| C34 | TMS request and dispatch codecs | C33 | local |
| C35 | TMS routed dispatch contract and receivers | C34 | claude |
| C36 | Base refinery ingot demand | C35 | claude |
