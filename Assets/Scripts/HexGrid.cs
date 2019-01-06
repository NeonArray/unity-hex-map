using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private int m_width = 6;
    [SerializeField] private int m_height = 6;
    private Canvas m_gridCanvas;
    private HexCell[] m_cells;
    private HexMesh m_hexMesh;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    private void Awake ()
    {
        this.m_gridCanvas = this.GetComponentInChildren<Canvas>();
        this.m_hexMesh = this.GetComponentInChildren<HexMesh>();
        this.m_cells = new HexCell[this.m_height * this.m_width];
        
        for (int z = 0, i = 0; z < this.m_height; z++)
        {
            for (int x = 0; x < this.m_width; x++)
            {
                this.CreateCell(x, z, i++);
            }
        }
    }

    private void Start ()
    {
        this.m_hexMesh.Triangulate(this.m_cells);
    }

    private void CreateCell (int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        HexCell cell = this.m_cells[i] = Instantiate(this.cellPrefab);
        cell.transform.SetParent(this.transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = this.defaultColor;
        
        Text label = Instantiate(this.cellLabelPrefab);
        label.rectTransform.SetParent(this.m_gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    private void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            this.HandleInput();
        }
    }

    private void HandleInput ()
    {
        Ray inputRay = FindObjectOfType<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            this.TouchCell(hit.point);
        }
    }

    private void TouchCell (Vector3 position)
    {
        Vector3 invertedPoint = this.transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(invertedPoint);
        int index = coordinates.X + coordinates.Z * this.m_width + coordinates.Z / 2;
        HexCell cell = this.m_cells[index];
        cell.color = this.touchedColor;
        this.m_hexMesh.Triangulate(this.m_cells);
    }
}
