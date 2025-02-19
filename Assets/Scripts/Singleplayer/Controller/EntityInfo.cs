
using Sirenix.OdinInspector;

[System.Serializable]
public class EntityInfo
{
    public string name;
    public EntityType entityType;
    [ShowIf(nameof(ShowMonsterType))] public MonsterType monsterType;
    //public int health;
    //public int maxHealth;

    private bool ShowMonsterType => entityType == EntityType.Monster;
}
