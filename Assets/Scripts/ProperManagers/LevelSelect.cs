using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using gc = GameController;

public class LevelSelect : MonoBehaviour
{
    ToggleGroupExtended tg;
    Toggle toggle;
    Toggle[] toggles;

    private void Awake()
    {
        tg = GetComponent<ToggleGroupExtended>();
    }

    private void Start()
    {
        toggles = tg.GetToggles().ToArray();
        GameObject comingSoon;

        for (int i = 0; i < toggles.Length; i++)
        {
            comingSoon = toggles[i].transform.Find("ComingSoon").gameObject;
            if (toggles[i].interactable) comingSoon.SetActive(false);
            else comingSoon.SetActive(true);
        }
    }

    public void PlayLevel()
    {
        toggle = tg.GetFirstActiveToggle();
        gc.Instance.OnLoadGameScene(toggle.name);
    }
}
