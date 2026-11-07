---
name: retrospective
description: Extract evidence-backed lessons from a finished milestone, stage, release or live update, and turn them into concrete changes to how the team (and these agents and skills) work. Use after each stage gate, release, or painful incident.
---

# Game Development Retrospective

## 1. Gather facts first

From `DECISIONS.md`, reviews, reports, git history and the issue list for the period:
- What was planned vs what shipped, and dates planned vs actual
- Bugs found late (in regression or by players) and where they could have been caught
- Decisions that were reversed, and why
- Metrics vs expectations (perf, retention, revenue, crash rate)

## 2. Ask four questions

1. **What worked** and should be kept deliberately?
2. **What hurt** (time lost, rework, bugs, stress), and what was the root cause, not the symptom?
3. **What surprised us**: assumptions that turned out wrong?
4. **What would we do differently** with what we know now?

Every answer needs evidence: a commit, a report, a number, a date.

## 3. Turn lessons into changes

Each lesson becomes one concrete action with an owner:
- A new check added to a skill (for example a regression test case, a release-gate checklist item)
- A convention added to `CLAUDE.md`
- A tunable or budget changed in `TUNING.md`
- A new test or bot scenario

If a lesson is about these agents or skills themselves (a check that was missing, a step that was wrong), edit that file in `.claude/` and note it.

## Output

`docs/gamedev/reviews/YYYY-MM-DD-retro-<milestone>.md`: facts, the four answers, and an action table (lesson → change → owner → done?). Keep it short and honest; no blame, only causes and changes.
