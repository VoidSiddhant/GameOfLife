using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    public int sizeX, sizeY;
    public GameObject cellPrefab;

    private Dictionary<Vector2, Cell> dic_cells;

    public bool generate = false;

	public delegate void RegisterCallback();
	public RegisterCallback FrameEndCallback;
    private float timeDelay = 0f;
    private float timeElapsed = 0f;
	public Slider slider;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        Generate();
    }

    void Generate()
    {
        int halfX = sizeX / 2;
        int halfY = sizeY / 2;
        dic_cells = new Dictionary<Vector2, Cell>();
        for (int rows = 0; rows < sizeY; rows++)
        {
            for (int cols = 0; cols < sizeX; cols++)
            {
                GameObject o = Instantiate(cellPrefab, new Vector2(cols, rows), Quaternion.identity);
                dic_cells.Add(o.transform.position, o.GetComponent<Cell>());
            }
        }
    }

    void FrameCheck()
    {
        Dictionary<Vector2, Cell>.KeyCollection keys = dic_cells.Keys;
        foreach (var key in keys)
        {
            Vector2 pos = key;
            Vector2 nextPos = new Vector2(pos.x + 1, pos.y);                //Right
            CheckNeighbours(dic_cells[key], nextPos);

            nextPos = new Vector2(pos.x, pos.y + 1);                        //Top
            CheckNeighbours(dic_cells[key], nextPos);

            nextPos = new Vector2(pos.x + 1, pos.y + 1);                    //Top-Right
            CheckNeighbours(dic_cells[key], nextPos);

            nextPos = new Vector2(pos.x - 1, pos.y + 1);					//Top-left
            CheckNeighbours(dic_cells[key], nextPos);
        }
    }

    /* RULES
	* Each Cell with 2 or 3 neighbours survive
	* Each Cell with more than 3 neighbours dies 
	* Each Cell with less than 2 neighbours dies
	* Each Empty Cell with exactly 3 neighbours becomes alive   */
    public void CheckNeighbours(Cell currentCell, Vector2 nextPos)
    {
        Cell nextCell = null;
        //Right
        if (dic_cells.TryGetValue(nextPos, out nextCell))
        {
            if (nextCell.IsAlive && currentCell.IsAlive)
            {
                currentCell.IncrementHealth();
                nextCell.IncrementHealth();
            }
            else if (!nextCell.IsAlive && currentCell.IsAlive)
            {
                nextCell.IncrementHealth();
            }
            else if (nextCell.IsAlive && !currentCell.IsAlive)
            {
                currentCell.IncrementHealth();
            }
        }
    }

    // Update is called once per frame
    
	void Update()
	{
		timeElapsed += Time.deltaTime;
		if(timeElapsed >= timeDelay)
		{
			UpdateFrame();
			timeElapsed = 0f;
		}
	}
	void UpdateFrame()
    {
        if (!generate)
        {
            FrameCheck();
			FrameEndCallback();
        }
    }

	public void NextFrame()
	{
		FrameCheck();
		FrameEndCallback();
	}

	public void PlayPause(UnityEngine.UI.Text text)
	{
		generate = !generate;
		text.text = (generate ? "Play" : "Pause");
	}

	public void SetTimeDelay()
	{
		timeDelay = slider.value ;
	}
}
