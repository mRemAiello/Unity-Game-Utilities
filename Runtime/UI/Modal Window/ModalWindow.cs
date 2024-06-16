using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModalWindow<T> : MonoBehaviour where T : ModalWindow<T>
{
    [SerializeField] private TextMeshProUGUI m_QuestionText;

    [Header("Buttons")]
    [SerializeField] private Button m_BackgroundButton;
    [SerializeField] private Transform m_ButtonsRoot;

    protected List<ModalWindowButton> m_Buttons = new List<ModalWindowButton>();

    // Simple resources cache
    static protected Dictionary<string, MonoBehaviour> m_ResourcesCache = new Dictionary<string, MonoBehaviour>();

    private Animator m_Animator;

    private bool m_Ignorable;

    public virtual bool Ignorable
    {
        get => m_Ignorable;
        protected set
        {
            m_Ignorable = value;

            if (m_BackgroundButton)
                m_BackgroundButton.interactable = value;
        }
    }

    public bool Visible
    {
        get => m_Animator.GetBool("Visible");
        set => m_Animator.SetBool("Visible", value);
    }

    protected T Instance { get; set; }

    private void Awake()
    {
        // Animator 
        m_Animator = GetComponent<Animator>();

        // Click event del bottone quit
        if (m_BackgroundButton)
            m_BackgroundButton.onClick.AddListener(new UnityEngine.Events.UnityAction(UI_IgnorePopup));
    }

    protected static ResType GetModalWindowResource<ResType>(string path) where ResType : MonoBehaviour
    {
        return null;
        /*var fullResourcesPath = path + " (Prefab)";
        if (m_ResourcesCache.TryGetValue(fullResourcesPath, out var res))
        {
            return res as ResType;
        }
        else
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(fullResourcesPath);
            handle.WaitForCompletion();

            var component = handle.Result.GetComponent<ResType>();
            m_ResourcesCache.Add(fullResourcesPath, component);

            return component;
        }*/
    }

    protected static ModalWindowButton GetButtonPrefab(ModalButtonType buttonType)
    {
        return GetModalWindowResource<ModalWindowButton>(buttonType.ToString());
    }

    public static T Create(bool ignorable = true)
    {
        var name = typeof(T).ToString();
        var modalWindow = Instantiate(GetModalWindowResource<T>(name));
        modalWindow.name = name;
        modalWindow.Ignorable = ignorable;
        modalWindow.Instance = modalWindow;

        return modalWindow.Instance;
    }

    public virtual void SetBody(string text)
    {
        m_QuestionText.text = text;
    }

    public virtual void AddButton(string text, Action action = null, ModalButtonType type = ModalButtonType.Normal)
    {
        if (!m_ButtonsRoot)
        {
            return;
        }

        /*var button = Instantiate(GetButtonPrefab(type), buttonsRoot);
        button.Init(ButtonPressedCallback, text, action, type);
        m_Buttons.Add(button);*/
    }

    protected virtual void OnBeforeShow()
    {
        if (m_Buttons.Count == 0)
        {
            AddButton("Ok");
        }
    }

    public virtual void Show()
    {
        OnBeforeShow();

        Visible = true;
        transform.SetAsLastSibling();
    }

    public virtual void Close()
    {
        Visible = false;
        Destroy(gameObject, 1f);
    }

    public virtual void UI_IgnorePopup()
    {
        if (Ignorable)
        {
            Close();
        }
    }

    public virtual void ButtonPressedCallback(ModalWindowButton modalWindowButton)
    {
        Close();
    }

    /*protected virtual void Update()
    {
        CheckIgnorableForClose();
    }

    protected virtual void CheckIgnorableForClose()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!ReferenceEquals(Instance, null))
            {
                if (Instance.ignorable)
                    Instance.Close();
            }
        }
    }*/
}