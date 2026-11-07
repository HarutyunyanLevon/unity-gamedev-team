#!/usr/bin/env bash
# Installs the unity-gamedev-team into a Unity project (macOS/Linux/Git Bash).
# Usage: ./install.sh /path/to/MyGame [--force]
set -euo pipefail
here="$(cd "$(dirname "$0")" && pwd)"
project="${1:?usage: ./install.sh /path/to/UnityProject [--force]}"
force="${2:-}"
dst="$project/.claude"

[ -d "$project/Assets" ] || echo "warning: no Assets folder in $project"

copied=0; kept=0
while IFS= read -r -d '' f; do
  rel="${f#$here/claude/}"
  target="$dst/$rel"
  mkdir -p "$(dirname "$target")"
  if [ -f "$target" ] && [ "$force" != "--force" ] && ! cmp -s "$f" "$target"; then
    echo "kept (edited):  $rel"; kept=$((kept+1)); continue
  fi
  cp "$f" "$target"; copied=$((copied+1))
done < <(find "$here/claude" -type f -print0)

md="$project/CLAUDE.md"
if [ -f "$md" ] && grep -q "unity-gamedev-team:start" "$md"; then
  python3 - "$md" "$here/CLAUDE.md" <<'EOF'
import re,sys
p,b=sys.argv[1],open(sys.argv[2]).read().strip()
t=open(p).read()
open(p,'w').write(re.sub(r'(?s)<!-- unity-gamedev-team:start -->.*?<!-- unity-gamedev-team:end -->',lambda m:b,t))
EOF
  echo "CLAUDE.md: block updated"
elif [ -f "$md" ]; then
  printf '\n' >> "$md"; cat "$here/CLAUDE.md" >> "$md"; echo "CLAUDE.md: block appended"
else
  cp "$here/CLAUDE.md" "$md"; echo "CLAUDE.md: created"
fi
echo "Done: $copied files installed, $kept kept. Open Claude Code in $project and run /stage."
