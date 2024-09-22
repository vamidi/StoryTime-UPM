using System;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace StoryTime.Domains.UI.Localization
{
    public class LocalizedButton : Button
    {
        // Must expose your element class to a { get; set; } property that has the same name 
        // as the name you set in your UXML attribute description with the camel case format
        public string Key { get; set; }
        
        public LocalizedButton()
        {
            schedule.Execute(() =>
            {
                if(!String.IsNullOrEmpty(Key))
                {
                    string loc = LocalizationSettings.StringDatabase.GetLocalizedString(Key);
                    if (!String.IsNullOrEmpty(loc))
                    {
                        text = loc;
                        return;
                    }

                    text = Key;
                }
            });
        }
        
        public new class UxmlFactory : UxmlFactory<LocalizedButton, UxmlTraits> {}
        
        public new class UxmlTraits : Button.UxmlTraits
        {
            UxmlStringAttributeDescription _key = new (){ name = "Key", defaultValue = "YOUR_LOCALIZED_KEY" };
            
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var localizedLabel = ve as LocalizedButton;
                
                if(localizedLabel == null) return;

                localizedLabel.Key = _key.GetValueFromBag(bag, cc);
            }
        }
    }
}