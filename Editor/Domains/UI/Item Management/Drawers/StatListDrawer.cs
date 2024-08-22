using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
#endif

namespace StoryTime.Editor.Domains.UI.Item_Management.Drawers
{
    // 
    // The StatList is a dictionary-like list of StatValues, which holds a StatType and a value.
    // This could be used by many things throughout the system. In this case, StatLists are used
    // by the Character and items to define requirements and modifiers. But one could imagine
    // that many things in a game could have StatLists.
    // 
    // The reason for it being a list instead of a dictioanry is, that most often StatLists doesn't 
    // contain very many stats. For instance, a shield might add some defences, and a few other random bonuses,
    // and iterating over a dozen values, is actually faster than making a dictionary lookup if optimized.
    // 
    // The StatList is then customized with the ValueDropdown attribute, where we override how elements 
    // are added and provide the user with a list of types to choose from using OdinSelectors. 
    // Checkout the CustomAddStatsButton at the bottom of this script.
    // 

    [Serializable]
    public class StatListDrawer
    {
        [SerializeField]
        [ValueDropdown("CustomAddStatsButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Stats")]
        [ListDrawerSettings(DraggableItems = false, ShowFoldout = true)]
        // private List<StatValue> stats = new ();

        // public StatValue this[int index]
        // {
            // get { return this.stats[index]; }
            // set { this.stats[index] = value; }
        // }

        // public int Count
        // {
            // get { return this.stats.Count; }
        // }
/*
        public float this[StatType type]
        {
            get
            {
                for (int i = 0; i < this.stats.Count; i++)
                {
                    if (this.stats[i].Type == type)
                    {
                        return this.stats[i].Value;
                    }
                }

                return 0;
            }
            set
            {
                for (int i = 0; i < this.stats.Count; i++)
                {
                    if (this.stats[i].Type == type)
                    {
                        var val = this.stats[i];
                        val.Value = value;
                        this.stats[i] = val;
                        return;
                    }
                }

                // this.stats.Add(new StatValue(type, value));
            }
        }
*/
#if UNITY_EDITOR
        // Finds all available stat-types and excludes the types that the statList already contains, so we don't get multiple entries of the same type.
        private IEnumerable CustomAddStatsButton()
        {
            // TODO fixme
            return new string[10];
            // return Enum.GetValues(typeof(StatType)).Cast<StatType>()
            // .Except(this.stats.Select(x => x.Type))
            // .Select(x => new StatValue(x))
            // .AppendWith(this.stats)
            // .Select(x => new ValueDropdownItem(x.Type.ToString(), x));
        }
#endif
    }

    // 
    // Since the StatList is just a class that contains a list, all StatLists would contain an extra 
    // label with a foldout in the inspector, which we don't want.
    // 
    // So with this drawer, we simply take the label of the member that holds the StatsList, and render the 
    // actual list using that label.
    //
    // So instead of the "private List<StatValue> stats" field getting a label named "Stats"
    // It now gets the label of whatever member holds the actual StatsList
    // 
    // If this confuses you, try out commenting the drawer below, and take a look at an item in the RPGEditor to see 
    // the difference.
    // 

    internal class StatListValueDrawer : OdinValueDrawer<StatListDrawer>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // This would be the "private List<StatValue> stats" field.
            this.Property.Children[0].Draw(label);
        }
    }
}