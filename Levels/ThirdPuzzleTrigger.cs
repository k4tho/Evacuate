using UnityEngine;

public class ThirdPuzzleTrigger : MonoBehaviour
{
    public TutorialLevel tutorialLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            tutorialLevel.UpdateState(TutorialLevel.TutorialState.ThirdPuzzle);
    }
}
