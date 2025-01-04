using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public float countdownTime = 40f;
    private float timer;

    [SerializeField] GameObject background;
    [SerializeField] GameObject cameraControls;
    [SerializeField] GameObject towerPlacementControls;
    [SerializeField] GameObject enemyTutorial;
    [SerializeField] GameObject currencyTutorial;
    [SerializeField] GameObject upgradeTutorial;
    [SerializeField] GameObject conditions;

    void Start()
    {
        timer = countdownTime;
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator WaitForTime()
    {
        yield return new WaitForSecondsRealtime(5f);
        DeactivateBackground();
        Time.timeScale = 1f;
        conditions.SetActive(false);
    }

    IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            Debug.Log($"CountdownCoroutine: Timer = {timer}");
            yield return new WaitForSeconds(1f);
            timer -= 1;

            if (Mathf.Approximately(timer, 37))
            {
                CameraControls();
            }

            if (Mathf.Approximately(timer, 30))
            {
                TowerPlacementControls();
            }

            if (Mathf.Approximately(timer, 24))
            {
                EnemyTutorial();
            }

            if (Mathf.Approximately(timer, 16))
            {
                CurrencyTutorial();
            }

            if (Mathf.Approximately(timer, 9))
            {
                UpgradeTutorial();
            }

            if (Mathf.Approximately(timer, 3))
            {
                Conditions();
            }
        }
    }

    void ActivateBackground()
    {
        background.SetActive(true);
    }

    void DeactivateBackground()
    {
        background.SetActive(false);
    }

    public void CameraControls()
    {
        ActivateBackground();
        cameraControls.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    public void TowerPlacementControls()
    {
        cameraControls.SetActive(false);
        towerPlacementControls.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    public void EnemyTutorial()
    {
        towerPlacementControls.SetActive(false);
        enemyTutorial.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    public void CurrencyTutorial()
    {
        ActivateBackground();
        enemyTutorial.SetActive(false);
        Time.timeScale = 0f;
        currencyTutorial.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    public void UpgradeTutorial()
    {
        currencyTutorial.SetActive(false);
        upgradeTutorial.SetActive(true);
        StartCoroutine(WaitForTime());
    }

    public void Conditions()
    {
        ActivateBackground();
        upgradeTutorial.SetActive(false);
        conditions.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(WaitForTime());
    }
}
