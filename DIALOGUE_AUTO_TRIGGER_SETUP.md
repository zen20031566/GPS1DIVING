# Auto-Triggered Dialogue Setup

## 🎯 Overview

This guide shows you how to set up dialogues that appear automatically based on game events (not NPC interaction).

---

## 📋 Setup Auto-Triggered Dialogues

### 1. Create Dialogue Manager (One Time Setup)

If you haven't already:

1. **Create Empty GameObject** in Hierarchy
2. **Name**: `DialogueManager`
3. **Add Component**: DialogueManager
4. **Create and assign DialogueUI** (see main instructions)

---

### 2. Create Auto Dialogue Trigger GameObject

For **each** auto-triggered dialogue you want:

1. **Right-click in Hierarchy** → Create Empty
2. **Name it** based on when it triggers:
   - `Dialogue_OnGameStart` (first dialogue when game loads)
   - `Dialogue_AfterFirstMove` (after player moves)
   - `Dialogue_QuestComplete_Tutorial` (after quest completes)
   - etc.

3. **Add Component** → `AutoDialogueTrigger`

---

## 🎮 Trigger Types & Configuration

### Type 1: **On Game Start** (First Dialogue)

**Use Case**: Show welcome message when game loads

**Setup**:
1. Select your AutoDialogueTrigger GameObject
2. In Inspector:
   ```
   Trigger Type: On Start
   Dialogue Data: [Drag your dialogue ScriptableObject]
   Delay Before Show: 0.5 (optional delay in seconds)
   ```

**Example**:
- GameObject: `Dialogue_GameIntro`
- Triggers immediately (or after delay) when scene loads
- Perfect for: Tutorial intro, story setup, controls explanation

---

### Type 2: **On Player Move** (Second Dialogue)

**Use Case**: Show dialogue after player explores a bit

**Setup**:
1. Select your AutoDialogueTrigger GameObject
2. In Inspector:
   ```
   Trigger Type: On Player Move
   Dialogue Data: [Drag your dialogue]
   Movement Threshold: 5.0 (distance player must move)
   ```

**Example**:
- GameObject: `Dialogue_AfterFirstMove`
- Triggers when player moves 5 units from starting position
- Perfect for: Follow-up instructions, encouragement to explore

---

### Type 3: **On Quest Start** (Before Quest)

**Use Case**: Show dialogue when a quest begins

**Setup**:
1. Select your AutoDialogueTrigger GameObject
2. In Inspector:
   ```
   Trigger Type: On Quest Start
   Dialogue Data: [Drag your dialogue]
   Quest Id: "tutorial_quest" (exact quest ID)
   Delay Before Show: 0
   ```

**Example**:
- GameObject: `Dialogue_QuestStart_Tutorial`
- Triggers when quest "tutorial_quest" starts
- Perfect for: Quest briefing, objective reminder

---

### Type 4: **On Quest Complete** (After Quest)

**Use Case**: Show dialogue when a quest is completed

**Setup**:
1. Select your AutoDialogueTrigger GameObject
2. In Inspector:
   ```
   Trigger Type: On Quest Complete
   Dialogue Data: [Drag your dialogue]
   Quest Id: "tutorial_quest" (exact quest ID)
   Delay Before Show: 0.5 (slight delay for polish)
   ```

**Example**:
- GameObject: `Dialogue_QuestComplete_Tutorial`
- Triggers when quest "tutorial_quest" completes
- Perfect for: Congratulations, story progression, next steps

---

## 📝 Example: Full Tutorial Dialogue Flow

### Dialogue 1: Welcome (On Game Start)

**GameObject**: `Dialogue_Welcome`

**Settings**:
```
Trigger Type: On Start
Delay Before Show: 1.0
```

**DialogueDataSO Content**:
```
Lines:
- "Welcome to the depths, young diver."
- "Your journey begins here."
- "Use WASD to move around."
```

---

### Dialogue 2: Movement Encouragement (After First Move)

**GameObject**: `Dialogue_AfterMove`

**Settings**:
```
Trigger Type: On Player Move
Movement Threshold: 3.0
```

**DialogueDataSO Content**:
```
Lines:
- "Good! You're getting the hang of it."
- "Now dive deeper to explore the ocean."
```

---

### Dialogue 3: Quest Complete (After Quest)

**GameObject**: `Dialogue_QuestDone`

**Settings**:
```
Trigger Type: On Quest Complete
Quest Id: "first_dive"
Delay Before Show: 0.5
```

**DialogueDataSO Content**:
```
Lines:
- "Well done! You completed your first dive."
- "The ocean has many more secrets to reveal..."
```

---

## 🔧 Advanced: Multiple Dialogues Sequence

### Chain Dialogues Together:

**Method 1: Use Delays**
```
Dialogue 1: OnStart, Delay = 0
Dialogue 2: OnStart, Delay = 10 (shows 10 sec after first)
```

**Method 2: Use Quest System**
```
Dialogue 1: OnStart
  ↓ (Starts Quest A)
Dialogue 2: OnQuestStart, QuestId = "quest_a"
  ↓ (Quest A completes)
Dialogue 3: OnQuestComplete, QuestId = "quest_a"
```

**Method 3: Custom Code**
```csharp
// In your script:
GameObject.Find("Dialogue_Custom").GetComponent<AutoDialogueTrigger>().ManualTrigger();
```

---

## ⚙️ Settings Explained

### **Dialogue Data**
- The ScriptableObject containing the dialogue lines
- Create in: Assets/Resources/Dialogues/

### **Trigger Type**
- **On Start**: Triggers when scene loads
- **On Player Move**: Triggers when player moves X distance
- **On Quest Start**: Triggers when specific quest begins
- **On Quest Complete**: Triggers when specific quest finishes
- **Manual**: Trigger from code

### **Movement Threshold**
- Only for "On Player Move" type
- Distance (in Unity units) player must move
- Measured from starting position

### **Quest Id**
- Only for quest-based triggers
- Must match Quest ID exactly (case-sensitive)
- Find in your QuestInfoSO

### **Delay Before Show**
- Seconds to wait before showing dialogue
- Useful for polish/timing
- Set to 0 for instant

---

## ✅ Testing Checklist

1. **Game Start Dialogue**:
   - Press Play
   - Dialogue should appear immediately (or after delay)

2. **Movement Dialogue**:
   - Press Play
   - Move around
   - Dialogue appears after moving threshold distance

3. **Quest Dialogues**:
   - Trigger quest start/complete
   - Dialogue appears automatically

4. **Progression**:
   - Complete dialogue
   - Player can move again
   - Next trigger works correctly

---

## 🎯 Your Specific Setup (Based on Requirements)

### 1st Dialogue: Game Start
```
GameObject: Dialogue_GameIntro
Trigger Type: On Start
Delay: 1.0
Content: Welcome message, game introduction
```

### 2nd Dialogue: After Player Moves
```
GameObject: Dialogue_FirstMovement
Trigger Type: On Player Move
Movement Threshold: 3.0
Content: Encouragement, next instructions
```

### 3rd+ Dialogues: Quest-Based
```
GameObject: Dialogue_Quest_[QuestName]
Trigger Type: On Quest Start OR On Quest Complete
Quest Id: [Your Quest ID]
Content: Quest briefing or completion message
```

---

## 🐛 Troubleshooting

**Dialogue doesn't appear on start:**
- Check Trigger Type is "On Start"
- Verify DialogueManager exists in scene
- Check Console for errors

**Movement trigger doesn't work:**
- Ensure Player has moved beyond Movement Threshold
- Check GameManager.PlayerTransform is assigned
- Try increasing Movement Threshold

**Quest triggers don't work:**
- Verify Quest ID matches exactly (case-sensitive)
- Check QuestManager is in scene
- Ensure quest actually starts/completes

**Multiple dialogues overlap:**
- Each dialogue should have its own AutoDialogueTrigger GameObject
- Only one triggers at a time (hasTriggered flag prevents repeats)
- Add delays between sequential dialogues

---

## 💡 Tips

1. **One GameObject per dialogue** - Each auto-trigger needs its own GameObject
2. **Use descriptive names** - Name GameObjects clearly (e.g., "Dialogue_AfterFirstDive")
3. **Test in sequence** - Test each trigger individually, then together
4. **Delays add polish** - Small delays (0.5-1s) make transitions smoother
5. **Don't destroy triggers** - Keep them in scene unless you want one-time-only (comment out the Destroy line in code)

---

Enjoy your auto-triggered dialogue system! 🎭
