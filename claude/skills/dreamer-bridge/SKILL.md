---
name: dreamer-bridge
description: Use when a game project gets its design from the Dreamer app (a docs/design/ pack and a dreamer:begin block in CLAUDE.md) - at session start, when a design question comes up while building, when a build-plan item is verified, and after a playtest. Keeps Dreamer the design source of truth and reports back into it.
---

# Dreamer Bridge

Dreamer is the owner's idea-canvas app. It holds the design and hands a versioned design pack into the game repo. The team builds from that pack and reports back only through files that Dreamer watches.

## What Dreamer puts in the repo

| Path | What it is | Team may |
|---|---|---|
| `docs/design/00-overview.md` … `05-assets.md`, `CHANGELOG.md`, `assets/` | Design pack version N (the top of `CHANGELOG.md` says which) | Read only. Replaced on every handoff |
| `docs/design/QUESTIONS.md` | Questions from the team to the owner | Append new `- [ ]` lines only |
| `<!-- dreamer:begin --> … <!-- dreamer:end -->` in `CLAUDE.md` | Dreamer's build instructions | Read only |

Precedence: the owner in chat beats an answered question in `QUESTIONS.md`, which beats the design pack, which beats anything the team inferred.

## How it maps to the team's files

- **`CONCEPT.md`** becomes a short pointer: "Design source: `docs/design/` (Dreamer vN)", plus the core verb and the riskiest assumption copied in for `stage-ladder`. A design pack meets the Concept stage exit criteria; cite `00-overview.md` and `01-gdd.md` as evidence.
- **`03-roadmap.md`** defines what's in the current version. Anything it marks as later is later-stage work for `stage-ladder`.
- **`DECISIONS.md`** holds engineering and process decisions only. Design decisions live in Dreamer: ask, don't decide.
- `STAGE.md`, `TUNING.md`, `CHEATS.md` and reports work as usual. Tunable values may change during tuning. If a tuned value contradicts a number in the design pack, ask a question with the measured result.

## Session start

1. Read the top of `docs/design/CHANGELOG.md`. If the version is newer than the one recorded in `STAGE.md`, read what changed and re-check the current tasks against it.
2. Read `QUESTIONS.md`. For every answered question (a `- [x]` line followed by an indented `- A:` line), apply the answer, remove the matching `// DESIGN-Q:` markers in code, and mention it in the session report.

## Running a Dreamer task

Newer Dreamer versions hand over one task file per card change instead of (or beside) a full pack. Recognize it by `.dreamer/link.json` in the repo; it names the paths:

| `link.json` key | Meaning |
|---|---|
| `tasks_dir` | Task files, for example `docs/design/tasks/YYYY-MM-DD-<slug>.md`. The frontmatter `cards:` lists the card ids the task builds |
| `status_file` | Build status, for example `docs/design/STATUS.md` |
| `outbox` | Folder Dreamer picks up report cards from (git-ignored with `.dreamer/`) |
| `cards[]` | Card id, title, stage, `build_status` and `dirty` flag. Read only |

1. Read the task's Goal, What to build, Acceptance criteria, Out of scope and Report back sections. On a rebuild, the "Design change since last build" diff is the only thing to apply; keep everything that already matches.
2. If something is missing or contradicts the repo, ask the owner in chat when they're present. Otherwise use a `QUESTIONS.md` line (below) with the most conservative option.
3. Commit once per logical step, each message starting with `[dreamer:<card id>]`.
4. Verify every acceptance item with evidence (compile log, test results, a grep), not assumption.
5. In `STATUS.md`, keep the first two lines exactly as they are. Add or replace one section per card, in the format the task's Report back gives (`## <card id> · <title>`, then `status`, `task`, `commits`, `files`, `evidence`, `notes`).
6. Report card: write one card per task to `<outbox>/<ULID>.md` in the card format below. Use `stage: reverie`, `source: claude`, tags from the task card plus `report`, and a link to the built card with `label: build report of`. The body holds the status, commits, the acceptance checklist with evidence, and open questions. Writing to the outbox is always allowed; it's not the Dreamer workspace.

## Asking a design question

Ask when the pack is silent, two docs contradict each other, or a TBD blocks the task. Engineering questions (which API, how to structure code) are the team's call; don't ask them.

1. Append one line to `docs/design/QUESTIONS.md`:
   `- [ ] Q (<task or file>): <question> — <context>; options: <A> / <B>; default used: <A>`
   One question per line, at most about 300 characters, no line breaks inside.
2. Don't block. Continue with the stated default and mark the code `// DESIGN-Q: <short question>`.
3. The owner answers in Dreamer on the "From Claude Code" card, and Dreamer rewrites the line as checked with an `- A:` line under it.
4. Never check boxes yourself, and never edit or delete lines that are already there.

## Writing into the Dreamer workspace

Only if the project's `CLAUDE.md` has both lines below, and only the two writes described here:

```
Dreamer project: <absolute path to the Dreamer project folder>
Dreamer build plan: cards/<card id>.md
```

Card files are Markdown with a YAML frontmatter between `---` lines. The file name stem is the card id. Dreamer reloads the folder live, so:
- re-read a file right before changing it;
- write atomically: write `.<name>.<random>.tmp` in the same folder, then rename it over the target;
- keep the frontmatter valid YAML and keep every field you don't own exactly as it was;
- set `updated:` to the current UTC time as `YYYY-MM-DDTHH:MM:SSZ`.

### a. Tick build-plan items

When a build-plan item is done **and verified** (its cheap check passed, with evidence), set `done: true` on that item in the build-plan card. Change nothing else: not the text, answers, ids, order, or any other card. If the work only partly covers an item, leave it unticked and say what's left in the session report.

### b. Playtest results card

After each playtest, create or update one card per version, titled `vN playtest results`:

```markdown
---
id: <ULID>
title: v1 playtest results
kind: card
stage: reverie
tags: [playtest, v1]
fields: {}
created: <UTC now>
updated: <UTC now>
source: claude
ghost: false
links:
- to: <build-plan card id>
  label: results of
attachments: []
---

## Session YYYY-MM-DD (build <version or commit>)
- Tester: <who>; unguided: yes/no
- Time to 100%: <mm:ss>; clicks per minute: <n>; research bought: <n>
- Stopped at: <time and reason>
- Notes: <what they said or did, quoted where possible>
```

- New card: file `cards/<ULID>.md`, where the id is a fresh 26-character uppercase Crockford-base32 ULID. If no ULID tool is installed, generate one with `python -c "import os,time;A='0123456789ABCDEFGHJKMNPQRSTVWXYZ';t=int(time.time()*1000);r=int.from_bytes(os.urandom(10),'big');print(''.join(A[(t>>(45-5*i))&31] for i in range(10))+''.join(A[(r>>(75-5*i))&31] for i in range(16)))"`. Don't add it to `canvas.json`; Dreamer places new cards itself.
- Later sessions append a new `## Session` section to the same card.
- Write facts and measurements, not design conclusions. The owner decides what they mean in Dreamer.

### Never

- Edit `memory.md`, other cards, chats, `canvas.json`, `project.json` or anything in `trash/`.
- Delete anything in the Dreamer workspace.
- Copy design pack text back into Dreamer cards.

## Next design version

When a version's build plan is done, or enough questions are answered that the pack is stale, tell the owner in the session report to run **Handoff → Generate** in Dreamer for the next version. The new pack replaces `docs/design/` (except `QUESTIONS.md`) and the Dreamer block in `CLAUDE.md`.
