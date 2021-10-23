using CombatSkills;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private TextMeshProUGUI skillName;
        private CombatingSkill _holdingSkill;


        public CombatingSkill GetSkill() => _holdingSkill;

        public void ForceUpdate()
        {
            skillName.text = _holdingSkill.GetSkillName();
        }

        public void Injection(USkillButtonsHolder holder) => _holder = holder;

        public void Injection(CombatingSkill skill)
        {
            _holdingSkill = skill;
            ForceUpdate();
        }

        public void ResetState()
        {
            _holdingSkill = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            RectTransform recTransform = transform as RectTransform;
            recTransform.DOScale(new Vector3(1.1f, 1.1f,1), .2f);
            _holder.OnHover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.one;
            _holder.OnHoverExit(this);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _holder.OnSelect(this);
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
            // todo Hide Selected
            // todo Update to cooldown
        }
    }
}
