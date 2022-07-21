using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIBaseHoverEntityHandler : UTeamElementSpawner<UUIBaseHoverEntityHolder>
    {
        [SerializeField,TitleGroup("References"),PropertyOrder(-10)] 
        private UHoverVitalityInfoHandler vitalityInfoHandler;

        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            vitalityInfoHandler.Injection(GetDictionary());
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }



        protected override void OnCreateElement(in CreationValues creationValues)
        {
            var element = creationValues.Element;
            var entity = creationValues.Entity;
            element.EntityInjection(entity);
            vitalityInfoHandler.UpdateEntityVitality(entity);
            element.Show();
        }
    }
}
