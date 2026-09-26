using NUnit.Framework;
using UnityEngine;

public class NoteUiManager : MonoBehaviour
{
    public GameObject notesSelectionUi;
    public GameObject selectedNoteUi;

    public GameObject bannonBookList;
    public GameObject bannonPhilosophicalNotes;
    public GameObject puzzleOneNoteOne;
    public GameObject puzzleOneNoteTwo;

    private bool puzzle1completed = false;
    public GameObject puzzleOneNoteOneNoteSelection;
    public GameObject puzzleOneNoteTWoNoteSelection;

    private GameManager2 gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager2>();

        ExitSpecificNote();
        ExitNoteSelection();
    }

    public void OpenNoteSelection()
    {
        notesSelectionUi.SetActive(true);
        selectedNoteUi.SetActive(false);

        if (puzzleOneNoteOneNoteSelection.activeSelf && puzzleOneNoteTWoNoteSelection.activeSelf)
        {
            puzzle1completed = true;
        }
    }

    public void CloseNoteSelection()
    {
        notesSelectionUi.SetActive(false);
    }

    public void CloseAllNotes()
    {
        bannonBookList.SetActive(false);
        bannonPhilosophicalNotes.SetActive(false);
        puzzleOneNoteOne.SetActive(false);
        puzzleOneNoteTwo.SetActive(false);
    }

    public void ExitNoteSelection()
    {
        CloseNoteSelection();
        gameManager.ShowPanel(GameManager2.PanelType.MainPanel);
    }

    public void ExitSpecificNote()
    {
        CloseAllNotes();
        selectedNoteUi.SetActive(false);
        OpenNoteSelection();

        DisableParentMask(puzzleOneNoteTwo);
        DisableParentMask(puzzleOneNoteOne);
        DisableParentMask(bannonPhilosophicalNotes);
        DisableParentMask(bannonBookList);
    }

    public void OpenBannonBookList()
    {
        CloseNoteSelection();
        CloseAllNotes();

        selectedNoteUi.SetActive(true);
        bannonBookList.SetActive(true);

        EnableParentMask(bannonBookList);
    }

    public void OpenBannonPhilosophicalNotes()
    {
        CloseNoteSelection();
        CloseAllNotes();
        selectedNoteUi.SetActive(true);
        bannonPhilosophicalNotes.SetActive(true);

        EnableParentMask(bannonPhilosophicalNotes);
    }

    public void OpenPuzzleOneNoteOne()
    {
        CloseNoteSelection();
        CloseAllNotes();
        selectedNoteUi.SetActive(true);
        puzzleOneNoteOne.SetActive(true);

        puzzleOneNoteTwo.SetActive(puzzle1completed);           // Open other puzzle piece too if it's completed

        EnableParentMask(puzzleOneNoteOne);
    }

    public void OpenPuzzleOneNoteTwo()
    {
        CloseNoteSelection();
        CloseAllNotes();
        selectedNoteUi.SetActive(true);
        puzzleOneNoteTwo.SetActive(true);

        puzzleOneNoteOne.SetActive(puzzle1completed);           // Open other puzzle piece too if it's completed

        EnableParentMask(puzzleOneNoteTwo);
    }

    private void EnableParentMask(GameObject obj)
    {
        GameObject parent = obj.GetComponent<Transform>().parent.gameObject;
        parent.SetActive(true);
        parent.GetComponent<NoteInspector>().enabled = true;
    }

    private void DisableParentMask(GameObject obj)
    {
        GameObject parent = obj.GetComponent<Transform>().parent.gameObject;
        parent.SetActive(false);
        parent.GetComponent<NoteInspector>().enabled = false;
    }
}
