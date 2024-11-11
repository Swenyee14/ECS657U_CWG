using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    public static TowerSelector selectedTower;
    public GameObject rangeIndicator;

    public TowerBehaviour towerBehaviour;

    void Awake()
    {
        towerBehaviour = GetComponent<TowerBehaviour>();
    }

    void OnMouseDown()
    {
        if (selectedTower == this)
        {
            // If it's already selected, deselect it and remove the indicator
            Destroy(rangeIndicator);
            selectedTower = null;
        }
        else
        {
            // Deselect the previous tower
            if (selectedTower != null && selectedTower.rangeIndicator != null)
            {
                Destroy(selectedTower.rangeIndicator);
            }

            // Set this tower as the selected tower and create a range indicator
            selectedTower = this;
            ShowRangeIndicator();
        }
    }

    private void ShowRangeIndicator()
    {
        if (towerBehaviour == null) return;

        // Create a sphere object to represent the range
        rangeIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rangeIndicator.transform.position = transform.position;
        rangeIndicator.transform.localScale = new Vector3(towerBehaviour.range * 2, 0.1f, towerBehaviour.range * 2);

        // Set the sphere to be transparent
        Material transparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        transparentMaterial.color = new Color(0, 0.5f, 1f, 0.4f); 
        rangeIndicator.GetComponent<Renderer>().material = transparentMaterial;

        // Disable collider on the range indicator to avoid interference with clicks
        Destroy(rangeIndicator.GetComponent<Collider>());

        // Make the range indicator a child of this tower for easy cleanup
        rangeIndicator.transform.SetParent(transform);
    }
}