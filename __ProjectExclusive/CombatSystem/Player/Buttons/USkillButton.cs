using CombatSkills;
using DG.Tweening;
using MPUIKIT;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace __ProjectExclusive.Player
{
    public class USkillButton : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private void Awake()
        {
            gameObject.SetActive(_holdingSkill != null);
        }

        private USkillButtonsHolder _holder;
        [Title("Tooltips")]
        [SerializeField] 
        private TextMeshProUGUI skillName;
        [Title("Visuals")]
        [SerializeField] 
        private Image icon;

        [SerializeField,HorizontalGroup("Background")] 
        private MPImage background;
        [SerializeField, HorizontalGroup("Background")] 
        private MPImage borderHolder;

        [SerializeField, HorizontalGroup("Cost")]
        private MPImage costHolder;
        [SerializeField, HorizontalGroup("Cost")]
        private TextMeshProUGUI costText;

        [ShowInInspector,ShowIf("_holdingSkill")]
        private CombatingSkill _holdingSkill;
        public CombatingSkill GetSkill() => _holdingSkill;

        public void ForceUpdate()
        {
            skillName.text = _holdingSkill.GetSkillName();
            UpdateCost();
        }
        public void UpdateCost()
        {
            int currentCost = _holdingSkill.GetUseCost();
            costText.text = UtilsText.GetSingleDigit(currentCost);
        }

        public void Injection(USkillButtonsHolder holder) => _holder = holder;

        public void Injection(CombatingSkill skill)
        {
            _holdingSkill = skill;
            ForceUpdate();
        }
        public void Injection(Sprite iconSprite, Color primaryColor)
        {
            icon.sprite = iconSprite;
            icon.color = primaryColor;

            borderHolder.color = primaryColor;
            costHolder.color = primaryColor;

        }

        public void ResetState()
        {
            _holdingSkill = null;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            HoverState();
            _holder.OnHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            InitialState();
            _holder.OnHoverExit(this);
        }

        private const float HoverAnimationDuration = .2f;
        private void HoverState()
        {
            RectTransform recTransform = transform as RectTransform;
            recTransform.DOLocalMoveY(12, HoverAnimationDuration);

        }

        private void InitialState()
        {
            var buttonTransform = transform;
            Vector3 localPosition = buttonTransform.localPosition;
            localPosition.y = 0;
            buttonTransform.localPosition = localPosition;
            buttonTransform.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _holder.OnSelect(this);
            RectTransform recTransform = transform as RectTransform;
            recTransform.DOPunchScale(new Vector3(.98f, .98f, 1), .2f, 2);
        }


        public void OnSelect()
        {
            // todo Show Selected
        }

        public void OnDeselect()
        {
            // todo Hide Selected
        }

        public void OnSubmit()
        {
            // todo submit (animation)
        }
    }
}
