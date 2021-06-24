
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeMonkey.Utils {
    
    /*
     * Button in the UI
     * */
    public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

        public Action ClickFunc = null;
        public Action MouseRightClickFunc = null;
        public Action MouseMiddleClickFunc = null;

        public enum HoverBehaviour {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;

        public Color hoverBehaviour_Color_Enter, hoverBehaviour_Color_Exit;
        public Image hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move = false;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector2 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        private bool mouseOver;
        private float mouseOverPerSecFuncTimer;

        private Action internalOnPointerEnterFunc, internalOnPointerExitFunc, internalOnPointerClickFunc;

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif


        public virtual void OnPointerEnter(PointerEventData eventData) {
            if (internalOnPointerEnterFunc != null) internalOnPointerEnterFunc();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();

            mouseOver = true;
            mouseOverPerSecFuncTimer = 0f;
        }
        public virtual void OnPointerExit(PointerEventData eventData) {
            if (internalOnPointerExitFunc != null) internalOnPointerExitFunc();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();

            mouseOver = false;
        }
        public virtual void OnPointerClick(PointerEventData eventData) {
            if (internalOnPointerClickFunc != null) internalOnPointerClickFunc();
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (triggerMouseOutFuncOnClick) {
                    OnPointerExit(eventData);
                }
                if (ClickFunc != null) ClickFunc();
            }
            if (eventData.button == PointerEventData.InputButton.Right)
                if (MouseRightClickFunc != null) MouseRightClickFunc();
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) MouseMiddleClickFunc();
        }
        public void Manual_OnPointerExit() {
            OnPointerExit(null);
        }
        public bool IsMouseOver() {
            return mouseOver;
        }
   
     
        void Awake() {
            posExit = transform.localPosition;
            posEnter = (Vector2)transform.localPosition + hoverBehaviour_Move_Amount;
        }

        public class InterceptActionHandler {

            private Action removeInterceptFunc;

            public InterceptActionHandler(Action removeInterceptFunc) {
                this.removeInterceptFunc = removeInterceptFunc;
            }
            public void RemoveIntercept() {
                removeInterceptFunc();
            }
        }
        public InterceptActionHandler InterceptActionClick(Func<bool> testPassthroughFunc) {
            return InterceptAction("ClickFunc", testPassthroughFunc);
        }
        public InterceptActionHandler InterceptAction(string fieldName, Func<bool> testPassthroughFunc) {
            return InterceptAction(GetType().GetField(fieldName), testPassthroughFunc);
        }
        public InterceptActionHandler InterceptAction(System.Reflection.FieldInfo fieldInfo, Func<bool> testPassthroughFunc) {
            Action backFunc = fieldInfo.GetValue(this) as Action;
            InterceptActionHandler interceptActionHandler = new InterceptActionHandler(() => fieldInfo.SetValue(this, backFunc));
            fieldInfo.SetValue(this, (Action)delegate () {
                if (testPassthroughFunc()) {
                    interceptActionHandler.RemoveIntercept();
                    backFunc();
                }
            });

            return interceptActionHandler;
        }
    }
}