# Enemy System Setup Instructions

## 🎯 What's Been Created

I've implemented a complete enemy/fish system with:
- AI state machine (Wander → Chase → Attack)
- Depth-based spawning system (5 depth layers)
- Player health/damage system
- Enemy events system

## 📋 Manual Unity Setup Required

### 1. Create Enemy Prefab Template

1. **Create a new GameObject** in your scene:
   - Right-click in Hierarchy → Create Empty
   - Name it: `EnemyFish_Template`

2. **Add Required Components**:
   - Add `Enemy` script (Assets/Scripts/Enemy/Enemy.cs)
   - Add `Rigidbody2D`:
     - Body Type: Dynamic
     - Gravity Scale: 0 (fish floats in water)
     - Linear Drag: 1-2 (for smooth movement)
     - Angular Drag: 1
     - Collision Detection: Continuous
   - Add `CircleCollider2D` or `BoxCollider2D`:
     - Is Trigger: False (we want collision for damage)
   - Add `SpriteRenderer`:
     - Sprite: Add your fish sprite
     - Sorting Layer: Enemies (create this if needed)

3. **Set the Tag**:
   - Select the GameObject
   - Tag dropdown → Add Tag → Create "Enemy" tag
   - Apply "Enemy" tag to your GameObject

4. **Save as Prefab**:
   - Drag the GameObject from Hierarchy to `Assets/Prefabs/` folder
   - Name it appropriately (e.g., "SmallFish", "DeepSeaCreature", etc.)
   - Delete the instance from Hierarchy

---

### 2. Setup Player Health System

1. **Select your Player GameObject** in the scene

2. **Add PlayerHealth Component**:
   - Add Script: PlayerHealth (should auto-appear if not already added)

3. **Assign References in Inspector**:
   - `Respawn Point`: Drag your respawn point Transform
   - `Health Bar_Filled`: Drag your health bar UI Image (create if needed)
   - `Damage Overlay`: Create a full-screen Red Image UI element (optional)
   - `Black Fade`: Should already exist (same one used by PlayerOxygen)
   - `Max Health`: 100 (default)
   - `Damage Flash Duration`: 0.3 (default)
   - `Fade Speed`: 2 (default)
   - `Invincibility Duration`: 1.5 (default)

4. **Create Health Bar UI** (if not exists):
   - Right-click Canvas → UI → Image
   - Name: `HealthBar_Background`
   - Position: Bottom-left or top-left corner
   - Set sprite to a bar background
   - Create child Image: `HealthBar_Filled`
     - Image Type: Filled
     - Fill Method: Horizontal
     - Fill Origin: Left
     - Color: Red
   - Assign `HealthBar_Filled` to PlayerHealth component

5. **Create Damage Overlay** (optional):
   - Right-click Canvas → UI → Image
   - Name: `DamageOverlay`
   - Anchor: Stretch to full screen
   - Color: Red with Alpha = 0
   - Assign to PlayerHealth component

---

### 3. Create Enemy Data ScriptableObjects

1. **Create Surface Fish**:
   - Right-click in `Assets/Resources/Enemies/` (create folder if needed)
   - Create → Enemy → Enemy Data
   - Name: `SmallFish_Surface`
   - Configure:
     - Enemy Name: "Small Fish"
     - Enemy Type: SmallFish
     - Health: 5
     - Damage: 10
     - Move Speed: 2
     - Chase Speed: 5
     - Detection Radius: 8
     - Attack Radius: 0.5
     - Depth Layer: Surface
     - Min Depth: 0
     - Max Depth: -20
     - Gold Reward: 5
     - Spawn Weight: 0.8
     - Prefab: Drag your fish prefab here
     - Sprite: Assign fish sprite

2. **Create Shallow Fish**:
   - Repeat above but with:
     - Name: `MediumFish_Shallow`
     - Enemy Type: MediumFish
     - Health: 15
     - Damage: 15
     - Move Speed: 2.5
     - Chase Speed: 6
     - Detection Radius: 10
     - Depth Layer: Shallow
     - Min Depth: -20
     - Max Depth: -40
     - Gold Reward: 10
     - Spawn Weight: 0.7

3. **Create Medium Depth Fish**:
   - Name: `LargeFish_Medium`
   - Enemy Type: LargeFish
   - Health: 25
   - Damage: 20
   - Move Speed: 3
   - Chase Speed: 7
   - Detection Radius: 12
   - Depth Layer: Medium
   - Min Depth: -40
   - Max Depth: -70
   - Gold Reward: 20
   - Spawn Weight: 0.6

4. **Create Deep Fish**:
   - Name: `DeepSeaCreature_Deep`
   - Enemy Type: DeepSeaCreature
   - Health: 40
   - Damage: 30
   - Move Speed: 3.5
   - Chase Speed: 8
   - Detection Radius: 15
   - Depth Layer: Deep
   - Min Depth: -70
   - Max Depth: -100
   - Gold Reward: 35
   - Spawn Weight: 0.5

5. **Create Abyss Creatures**:
   - Name: `AbyssHorror_Abyss`
   - Enemy Type: DeepSeaCreature
   - Health: 60
   - Damage: 50
   - Move Speed: 4
   - Chase Speed: 10
   - Detection Radius: 20
   - Depth Layer: Abyss
   - Min Depth: -100
   - Max Depth: -200
   - Gold Reward: 50
   - Spawn Weight: 0.3

---

### 4. Setup Enemy Spawner

1. **Create Spawner GameObject**:
   - Right-click in Hierarchy → Create Empty
   - Name: `EnemySpawner`

2. **Add EnemySpawner Component**

3. **Configure in Inspector**:
   - `Spawn Parent`: Create empty GameObject "Enemies" for organization (optional)
   - `Max Enemies Per Layer`: 10 (adjust based on performance)
   - `Spawn Check Interval`: 5 seconds
   - `Spawn Radius`: 30 (how far around player to spawn)
   - `Min Spawn Distance`: 15 (min distance from player)
   - `Min Spawn Bounds`: Your world minimum bounds (e.g., -100, -150)
   - `Max Spawn Bounds`: Your world maximum bounds (e.g., 100, 10)

4. **Assign Enemy Data Arrays**:
   - `Surface Enemies`: Drag all Surface enemy ScriptableObjects here
   - `Shallow Enemies`: Drag all Shallow enemy ScriptableObjects here
   - `Medium Enemies`: Drag all Medium depth enemy ScriptableObjects here
   - `Deep Enemies`: Drag all Deep enemy ScriptableObjects here
   - `Abyss Enemies`: Drag all Abyss enemy ScriptableObjects here

---

### 5. Setup Layers & Collision Matrix

1. **Create Layers** (Edit → Project Settings → Tags and Layers):
   - Add Layer: "Enemy"
   - Add Layer: "Player"

2. **Assign Layers**:
   - Select all enemy prefabs → Layer: Enemy
   - Select Player → Layer: Player

3. **Setup Collision Matrix** (Edit → Project Settings → Physics 2D):
   - Enemy should collide with: Player, Ground
   - Player should collide with: Enemy, Ground

---

### 6. Testing

1. **Enter Play Mode**
2. **Swim to different depths** and watch enemies spawn
3. **Get close to fish** (within detection radius) and they should chase
4. **Get hit** and watch health decrease with damage flash
5. **Check Scene View** with spawner selected to see Gizmos (spawn radius visualization)

---

## 🎨 Art Assets Needed

For each depth layer, create/find sprites:
- **Surface**: Small tropical fish (goldfish, neon tetras)
- **Shallow**: Medium fish (angelfish, grouper)
- **Medium**: Large predator fish (barracuda, sharks)
- **Deep**: Deep sea creatures (anglerfish, viperfish)
- **Abyss**: Horror creatures (giant squid, unknown horrors)

Scale sprites accordingly - surface fish should be smallest, abyss creatures largest!

---

## 🔧 Customization Tips

### Make Fish More Dangerous:
- Increase `Chase Speed` in EnemyDataSO
- Increase `Detection Radius`
- Decrease `Chase Time` so they pursue longer

### Adjust Spawn Rates:
- Change `Spawn Check Interval` (lower = more frequent spawns)
- Change `Max Enemies Per Layer` (higher = more enemies)
- Adjust `Spawn Weight` per enemy (higher = more common)

### Depth Customization:
Edit depth ranges in `EnemySpawner.cs` → `GetDepthRange()` method

---

## ⚠️ Important Notes

1. **Player Tag**: Make sure Player GameObject has "Player" tag
2. **Water Level**: Ensure `PlayerController.WaterLevel` is set correctly
3. **Spawn Bounds**: Adjust based on your actual map size
4. **Performance**: If lag occurs, reduce `maxEnemiesPerLayer` or increase `spawnCheckInterval`

---

## 🐛 Troubleshooting

**Enemies not spawning?**
- Check EnemySpawner has enemy data assigned
- Verify spawn bounds contain your player
- Check Scene view with spawner selected (Gizmos should show)

**Enemies not chasing?**
- Check Player tag is set
- Verify Detection Radius is large enough
- Check Enemy prefab has Enemy script attached

**No damage on hit?**
- Verify Player has PlayerHealth component
- Check colliders are not set to Trigger
- Confirm layers are setup correctly in Collision Matrix

**Fish flying away?**
- Set Rigidbody2D Gravity Scale to 0
- Add Linear Drag (1-2)
