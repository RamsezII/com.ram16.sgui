using _UTIL_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SoftwareButton : OSButton, IPointerDownHandler
    {
        [HideInInspector] public RectTransform rt;
        [HideInInspector] public Button button;
        [HideInInspector] public TMP_Dropdown dropdown;
        RawImage[] img_instances;

        public readonly ListListener<SguiWindow> instances = new();

        public Type software_type;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            rt = (RectTransform)transform;
            button = GetComponent<Button>();
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            instances.AddListener2(this, list =>
            {
                for (int i = 0; i < img_instances.Length; i++)
                    img_instances[i].gameObject.SetActive(list.Count > i);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (software_type == null)
            {
                Debug.LogWarning($"{nameof(software_type)} is null ({software_type})");
                return;
            }

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (instances.IsEmpty)
                    {
                        SguiWindow instance = SguiWindow.InstantiateWindow(software_type, true, true, true);

                        switch (instance)
                        {
                            case SguiWindow1 w1:
                                w1.SetScalePivot(this);
                                break;
                        }
                    }
                    else
                        for (int i = 0; i < instances._collection.Count; i++)
                        {
                            SguiWindow instance = instances._collection[i];
                            instance.SetScalePivot(this);
                            instance.ToggleWindow(true);
                        }
                    break;

                case PointerEventData.InputButton.Middle:
                    break;

                case PointerEventData.InputButton.Right:
                    dropdown.Show();
                    break;
            }
        }
    }
}