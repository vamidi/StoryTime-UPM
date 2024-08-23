using System;
using UnityEngine;

namespace StoryTime.Domains.Narrative.Stories.Services
{
    using Database.Binary;
    using ScriptableObjects;
    
    public class StoryService
    {
        public StorySO ConvertRow(TableRow row, StorySO scriptableObject = null)
        {
            StorySO story = scriptableObject ? scriptableObject : ScriptableObject.CreateInstance<StorySO>();

            if (row.Fields.Count == 0)
            {
                return story;
            }

            // story.ID = row.RowId;

            foreach (var field in row.Fields)
            {
                if (field.Key.Equals("id"))
                {
                    string data = (String) field.Value.Data;
                    // story.ID = data;
                }

                // Fetch the first dialogue we should start
                if (field.Key.Equals("childId"))
                {
                    // retrieve the necessary items
                    String data = (String) field.Value.Data;
                    // story.childId = data;
                }

                if (field.Key.Equals("parentId"))
                {
                    // retrieve the necessary items
                    String data = field.Value.Data;
                    // story.parentId = data;
                }

                if (field.Key.Equals("typeId"))
                {
                    // story.typeId = (StoryType) field.Value.Data;
                }
            }

            return story;
        }
    }
}