# Dialogue System Setup Instructions

## 🎯 What's Been Created

A complete dialogue system with:
- Character dialogue with typing animation
- Speaker names and portraits
- Quest integration (start/complete quests through dialogue)
- Conditional dialogues based on quest state
- Event system for dialogue tracking

---

## 📋 Unity Setup Required

### 1. Create Dialogue Manager GameObject

1. **Create Empty GameObject** in scene:
   - Right-click Hierarchy → Create Empty
   - Name: `DialogueManager`

2. **Add DialogueManager Component**:
   - Select DialogueManager GameObject
   - Add Component → `DialogueManager`

---

### 2. Create Dialogue UI

#### A. Create Dialogue Box Panel

1. **Right-click Canvas** → UI → Panel
2. **Name**: `DialogueBox`
3. **Configure**:
   - Anchor: Bottom (stretch across bottom of screen)
   - Position: Y = 100-150 (above bottom edge)
   - Height: 150-200
   - Background color: Semi-transparent black (0, 0, 0, 200)

#### B. Add Speaker Name Text

1. **Right-click DialogueBox** → UI → Text - TextMeshPro
2. **Name**: `SpeakerName`
3. **Configure**:
   - Anchor: Top-left of DialogueBox
   - Position: X = 20, Y = -20
   - Font Size: 24
   - Color: Yellow or White
   - Font Style: Bold

#### C. Add Dialogue Text

1. **Right-click DialogueBox** → UI → Text - TextMeshPro
2. **Name**: `DialogueText`
3. **Configure**:
   - Anchor: Fill (stretch to fit DialogueBox with margins)
   - Margins: Left = 20, Right = 20, Top = 60, Bottom = 40
   - Font Size: 18-20
   - Color: White
   - Alignment: Top-Left
   - Enable **Word Wrapping**

#### D. Add Speaker Portrait (Optional)

1. **Right-click DialogueBox** → UI → Image
2. **Name**: `SpeakerPortrait`
3. **Configure**:
   - Anchor: Left side of DialogueBox
   - Position: X = 20, Y = 0
   - Size: 80x80 (or 100x100)
   - Adjust DialogueText left margin to accommodate portrait

#### E. Add Continue Indicator

1. **Right-click DialogueBox** → UI → Image or Text
2. **Name**: `ContinueIndicator`
3. **Configure**:
   - Place in bottom-right corner of DialogueBox
   - Set sprite to an arrow (▼) or text "Press E"
   - Add Animator for blinking/pulsing animation (optional)

---

### 3. Create DialogueUI Component

1. **Select DialogueBox GameObject**
2. **Add Component** → `DialogueUI`
3. **Assign References** in Inspector:
   - **Dialogue Box**: Drag DialogueBox GameObject
   - **Speaker Name Text**: Drag SpeakerName TMP_Text
   - **Dialogue Text**: Drag DialogueText TMP_Text
   - **Speaker Portrait**: Drag SpeakerPortrait Image (if created)
   - **Continue Indicator**: Drag ContinueIndicator GameObject
   - **Speaker Panel**: Optional - parent object containing name/portrait

4. **Settings**:
   - **Hide Speaker If Empty**: Check if you want to hide name when not speaking

---

### 4. Link DialogueUI to DialogueManager

1. **Select DialogueManager** GameObject in Hierarchy
2. **In Inspector**, find **DialogueManager** component
3. **Assign**:
   - **Dialogue UI**: Drag the DialogueBox GameObject (with DialogueUI component)

---

### 5. Create Dialogue Data ScriptableObjects

#### Example: Create NPC Greeting Dialogue

1. **Right-click in Project** → `Assets/Resources/Dialogues/` (create folder)
2. **Create → Dialogue → Dialogue Data**
3. **Name**: `NPC_Greeting`

4. **Configure in Inspector**:
   ```
   Dialogue Id: "npc_greeting_01"

   Lines (Size: 3)
   ├─ Element 0:
   │  ├─ Text: "Hello there, diver! Welcome to our village."
   │  ├─ Speaker Name: "Fisherman Bob"
   │  └─ Speaker Portrait: [Assign portrait sprite if you have one]
   │
   ├─ Element 1:
   │  ├─ Text: "The waters have been dangerous lately..."
   │  ├─ Speaker Name: "Fisherman Bob"
   │  └─ Speaker Portrait: [Same portrait]
   │
   └─ Element 2:
      ├─ Text: "Be careful down there!"
      ├─ Speaker Name: "Fisherman Bob"
      └─ Speaker Portrait: [Same portrait]

   Conditions:
   ├─ Requires Quest: [Unchecked for basic dialogue]

   Actions:
   ├─ Starts Quest: [Unchecked]
   └─ Completes Quest: [Unchecked]
   ```

#### Example: Quest Starter Dialogue

1. **Create new Dialogue Data**: `NPC_QuestStart`
2. **Configure**:
   ```
   Dialogue Id: "npc_quest_start"

   Lines:
   ├─ "I need your help! Strange creatures have appeared."
   ├─ "Can you investigate the deep waters for me?"
   └─ "I'll reward you handsomely!"

   Actions:
   ├─ Starts Quest: [Checked]
   └─ Quest To Start Id: "investigate_creatures"
   ```

---

### 6. Create NPC with Dialogue Trigger

#### A. Create NPC GameObject

1. **Right-click Hierarchy** → Create Empty
2. **Name**: `NPC_Fisherman`
3. **Add Components**:
   - **Sprite Renderer** (assign NPC sprite)
   - **Collider2D** (Box or Circle)
     - **Is Trigger**: ✓ Checked
   - **DialogueTrigger** script

#### B. Configure DialogueTrigger

1. **Select NPC** GameObject
2. **In DialogueTrigger** component:
   - **Dialogue Data**: Drag your DialogueDataSO
   - **Interact Prompt**: "Press E to Talk"
   - **Interact Key**: E (or your preferred key)
   - **Interact Indicator**: Optional - create UI element above NPC

#### C. Optional: Add Interact Indicator

1. **Right-click NPC** → UI → Canvas (if no world space canvas exists)
2. Set Canvas to **World Space**
3. Add UI element (Text or Image) showing "E"
4. Position above NPC
5. Assign to DialogueTrigger's **Interact Indicator** field

---

### 7. Testing

1. **Press Play**
2. **Walk up to NPC** (trigger collider range)
3. **Press E** to start dialogue
4. **Press E** to advance through lines
5. **Press E** during typing to skip animation
6. Dialogue should end automatically after last line

---

## 🎨 Customization

### Typing Speed
Edit `DialogueManager.cs` line ~85:
```csharp
yield return new WaitForSeconds(0.03f); // Change this value
```
- Lower = Faster typing (0.01f)
- Higher = Slower typing (0.05f)

### Dialogue Box Appearance
- Adjust colors, fonts, sizes in UI elements
- Add background sprite for DialogueBox
- Add border/frame around dialogue box

### Speaker Portraits
- Create square sprites (100x100 or 128x128)
- Assign to DialogueLine.SpeakerPortrait field
- Portraits swap automatically per line

---

## 🔧 Advanced Features

### Conditional Dialogue (Quest-Based)

```csharp
// In DialogueDataSO:
Conditions:
├─ Requires Quest: [Checked]
├─ Required Quest Id: "tutorial_quest"
└─ Required Quest State: IN_PROGRESS

// This dialogue only shows if player has
// "tutorial_quest" in IN_PROGRESS state
```

### Dialogue Actions

```csharp
Actions:
├─ Starts Quest: [Checked]
├─ Quest To Start Id: "new_quest_01"
├─ Completes Quest: [Checked]
└─ Quest To Complete Id: "old_quest"

// Dialogue can start AND complete quests
```

### Multiple Dialogues Per NPC

Create multiple DialogueDataSO files and switch them based on:
- Quest progress
- Time of day
- Player stats
- Previous conversations

Use code to swap the DialogueTrigger's dialogueData field.

---

## 📝 Example Dialogue Structures

### Simple Greeting
```
Lines: 1-2 short lines
No conditions
No actions
Purpose: Flavor text, world-building
```

### Quest Giver
```
Lines: 3-5 explaining quest
Condition: Quest not started
Action: Starts quest
Purpose: Initiate player objectives
```

### Quest Complete
```
Lines: 2-3 thanking player
Condition: Quest in CAN_FINISH state
Action: Completes quest
Purpose: Reward player, progress story
```

### Story Progression
```
Lines: 5-10 story beats
Condition: Previous quest FINISHED
Action: Starts next quest
Purpose: Drive narrative forward
```

---

## ⚠️ Important Notes

1. **Player Input**: Make sure `InputManager.InteractPressed` is set up (or modify DialogueManager to use your input system)

2. **Quest Integration**: Dialogue actions require QuestManager to be present in scene

3. **Dialogue IDs**: Keep them unique! Use naming convention:
   - `npc_name_topic` (e.g., "bob_greeting", "bob_quest_start")

4. **UI Layering**: Ensure DialogueBox is on top of other UI (higher Sorting Order)

5. **Performance**: Typing coroutine is lightweight, but avoid triggering multiple dialogues simultaneously

---

## 🐛 Troubleshooting

**Dialogue doesn't start:**
- Check DialogueManager has DialogueUI assigned
- Verify NPC has Collider2D with Is Trigger checked
- Ensure NPC has Player tag in trigger range

**Text doesn't appear:**
- Check TMP_Text components are assigned
- Verify text color isn't same as background
- Check text is within DialogueBox bounds

**Can't progress dialogue:**
- Verify InputManager.InteractPressed is working
- Check console for errors
- Ensure Player is in OnUIOrDialog state

**Quest actions don't work:**
- Verify Quest IDs match exactly (case-sensitive)
- Check QuestManager exists in scene
- Ensure quests are loaded in QuestManager

---

## 🎮 Controls

- **E** - Advance dialogue / Skip typing
- **ESC** - Force end dialogue (optional, can be added)

---

Enjoy your new dialogue system! 🎭
