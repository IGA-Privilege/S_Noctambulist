using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeLockUnit : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private Image frame;
    private CodeLock _codeLock;
    public int Number;

    private void Awake()
    {
        _codeLock = GetComponentInParent<CodeLock>();
        Number = 0;
    }

    private void Update()
    {
        numberText.text = Number.ToString();

        if (_codeLock.SelectedUnit == this)
        {
            frame.color = Color.blue;
        }
        else
        {
            frame.color = Color.cyan;
        }
    }


    public void NumberIncrease()
    {
        Number++;
        if (Number > 9)
        {
            Number = 0;
        }
    }

    public void NumberDecrease()
    {
        Number--;
        if (Number < 0)
        {
            Number = 9;
        }
    }
}
