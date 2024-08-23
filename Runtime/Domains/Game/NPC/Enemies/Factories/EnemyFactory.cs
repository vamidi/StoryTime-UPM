
using UnityEngine;

namespace StoryTime.Domains.Game.NPC.Enemies.Factories
{
    using ScriptableObjects;
    using StoryTime.Domains.Database.Binary;

    public class EnemyFactory
    {
        public EnemySO ConvertRow(TableRow row, EnemySO scriptableObject = null)
        {
            EnemySO enemy = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<EnemySO>();

            if (row.Fields.Count == 0)
            {
                return enemy;
            }
			
			

            foreach (var field in row.Fields)
            {
                if (field.Key.Equals("id"))
                {
                    enemy.ID = uint.Parse(field.Value.Data);
                }

                if (field.Key.Equals("category"))
                {
                    enemy.Category = new EnemySO.EnemyCategory
                    {
                        categoryId = (uint)field.Value.Data
                    };
                }

                if (field.Key.Equals("name"))
                {
                    // TODO fixme
                    // enemy.EnemyName = (string) field.Value.Data;
                }
            }

            return enemy;
        }
    }
}