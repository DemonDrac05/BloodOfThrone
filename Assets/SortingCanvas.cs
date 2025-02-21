using UnityEngine;
using UnityEngine.UI;

public class SortingCanvas : MonoBehaviour
{
    [SerializeField] private Transform referenceObject;
    [SerializeField] private Canvas canvas;

    private void Update()
    {
        SortUI();
    }

    void SortUI()
    {
        if (referenceObject == null) return;
        Transform childTransform;

        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            childTransform = canvas.transform.GetChild(i);


            childTransform.SetSiblingIndex(0);
        }
    }
}