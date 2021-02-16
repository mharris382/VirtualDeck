using System;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    public class ScrollInt : MonoBehaviour
    {
       [Required, ChildGameObjectsOnly] public Image targetImage;
       [Required, ChildGameObjectsOnly] public TMPro.TextMeshProUGUI targetText;
        public int defaultValue = 1;

        [MinMaxSlider(-100, 100, showFields: true)]
        public Vector2Int range = new Vector2Int(-100, 100);


        private int currentValue;


        public int CurrentValue => currentValue;
        [InfoBox("$msg")]
        public string msgFormat = "hp+{0}";


        private string msg => string.Format(msgFormat, currentValue);
        public bool resetOnTurn;
        public EntityTestHelper testHelper;
        private void Awake()
        {
            if (resetOnTurn)
            {
                MessageBroker.Default.Receive<NewTurnMessage>().TakeUntilDestroy(this).Subscribe(_ =>
                {
                    SetValue(defaultValue);
                });
            }
            if(testHelper == null)
                testHelper = GetComponent<EntityTestHelper>();
            targetImage.OnScrollAsObservable()
                    .TakeUntilDestroy(this)
                    .ThrottleFrame(1, FrameCountType.Update)
                    .Subscribe(scrollData =>
                    {
                        if (scrollData.scrollDelta.y > 0)
                        {
                            SetValue(currentValue + 1);
                        }
                        else if (scrollData.scrollDelta.y < 0)
                        {
                            SetValue(currentValue - 1);
                        }
                        else
                        {
                            Debug.Log($"ScrollDelta: {scrollData.scrollDelta.ToString()}");
                        }
                    });

            targetImage.OnPointerClickAsObservable().TakeUntilDestroy(this)
                    .Subscribe(t =>
                    {
                        string m = msgFormat;
                        int v = currentValue;
                        if (currentValue < 0)
                        {
                            m = m.Replace('+', '-');
                        }

                        v = Mathf.Abs(currentValue);
                        testHelper.ExecuteOperationOnEntity(string.Format(m, v));
                    });
            currentValue = defaultValue;
            SetValue(defaultValue);
        }


        void SetValue(int value)
        {
            currentValue = Mathf.Clamp(value, range.x, range.y);
            targetText.text = value.ToString();
        }
    }
}