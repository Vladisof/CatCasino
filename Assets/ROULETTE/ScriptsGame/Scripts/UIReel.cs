using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

namespace Cat
{

  public class UIReel : MonoBehaviour
  {

    private Transform _tr;
    public SlotGame game;
    private bool _inSpin = false;
    private bool _inStop = false;

    public int ID;
    public Vector3 [] SymbolPos;
    public GameObject [] Symbols;
    [FormerlySerializedAs("SymbolFC")]
    public GameObject symbolFc = null;
    private bool _bWildFcExist = false;

    [HideInInspector]
    public int [] FinalValues;

    private float _speed;
    public float MinimumRotateDistance;
    public bool bSlotIndicateFirst;
    public int StopOffset = -2;
    private bool InDamping = false;
    private int SpinCount = 0;


    public void Init (SlotGame script, int x)
    {
      _tr = transform;
      game = script; //tr.parent.GetComponent<SlotGame>();
      ID = x;

      Symbols = new GameObject[game.RowCount + 1];
      SymbolPos = new Vector3[game.RowCount + 1];
      FinalValues = new int[game.RowCount];

      for (int y = 0; y < game.RowCount + 1; ++y)
      {
        GameObject go = BenObjectPool.Spawn(game.Symbols[0].prfab);
        go.transform.SetParent(_tr);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.name = "Symbol" + y.ToString();
        Symbols[y] = go;
      }
    }

    private void Awake()
    {
      _tr = transform;
    }

    void Start() {}

    private void Update()
    {

      if (!_inSpin)
        return;

      float dt = Mathf.Min(Time.deltaTime, 0.1f);
      Vector3 vPos = _tr.localPosition;

      if (_inStop && (MinimumRotateDistance <= 0.0f))
      {

        float fDelta = _speed * dt;

        if (StopOffset < game.RowCount)
        {
          vPos.y -= fDelta;

          if (vPos.y < -game.SymbolSizePlusMargin)
          {
            vPos.y += game.SymbolSizePlusMargin;

            if (symbolFc != null)
            {
              Vector3 vPosFC = symbolFc.transform.localPosition;
              vPosFC.y -= game.SymbolSizePlusMargin;
              symbolFc.transform.localPosition = vPosFC;
            }

            if (StopOffset >= -1)
            {
              SymbolShiftDown((StopOffset < game.RowCount - 1) ? FinalValues[StopOffset + 1] : GetRandom());
            } else
            {
              SymbolShiftDown(GetRandom());
            }

            StopOffset++;
          }
        } else
        {
          if (!InDamping)
          {
            vPos.y -= fDelta;

            if (vPos.y < -game.DampingHeight)
            {
              InDamping = true;
            }
          } else
          {
            if ((int)vPos.y != 0)
            {
              vPos.y = Mathf.Lerp(vPos.y, 0.0f, game.BoundSpeed * Time.deltaTime);
            } else
            {
              vPos.y = 0.0f;
              _inStop = false;
              _inSpin = false;
            }
          }
        }
      } else
      {
        _speed += game.Acceleration * dt;

        if (_speed >= game.SpeedMax)
          _speed = game.SpeedMax;

        float fDelta = _speed * dt;
        vPos.y -= fDelta;

        if (_inStop)
          MinimumRotateDistance -= fDelta;

        if (vPos.y < -game.SymbolSizePlusMargin)
        {
          vPos.y += game.SymbolSizePlusMargin;

          if (symbolFc != null)
          {
            Vector3 vPosFC = symbolFc.transform.localPosition;
            vPosFC.y -= game.SymbolSizePlusMargin;
            symbolFc.transform.localPosition = vPosFC;
          }

          SymbolShiftDown(GetRandom());
        }
      }

      _tr.localPosition = vPos;
    }

    private int GetRandom()
    {
      return UnityEngine.Random.Range(0, game.Symbols.Count);
    }

    public void SetSymbolRandom()
    {
      for (int y = 0; y < game.RowCount + 1; ++y)
      {
        SetSymbol(y, GetRandom());
      }
    }

    public void SetSymbol (int y, int Value)
    {
      if (Symbols[y] != null)
      {
        BenObjectPool.Unspawn(Symbols[y]);
      }

      //Debug.Log ("SetSymbol y:"+y+" Value:"+Value);
      Symbol newSymbol = game.GetSymbol(Value);

      //Debug.Log ("newSymbol "+newSymbol);
      if (newSymbol.type == SymbolType.WildFC)
        newSymbol = game.GetSymbolByType(SymbolType.Wild);

      GameObject go = BenObjectPool.Spawn(newSymbol.prfab);
      //Debug.Log ("go "+go);
      go.transform.SetParent(_tr);
      go.transform.localScale = Vector3.one;
      go.transform.localPosition = SymbolPos[y];
      Symbols[y] = go;
    }

    public void SymbolScaleReset()
    {
      for (int y = 0; y < game.RowCount + 1; ++y)
      {
        Symbols[y].transform.localScale = Vector3.one;
      }
    }

    public void SymbolShiftDown (int addedValue)
    {

      SpinCount++;

      if (_bWildFcExist && (symbolFc == null) && (StopOffset == -1))
      {

        Symbol symbolFC = game.GetSymbolByType(SymbolType.WildFC);

        if (symbolFC != null)
        {

          float yOffset = game.SymbolSizePlusMargin * (float)(game.RowCount - 1) * 0.15f;

          GameObject go = BenObjectPool.Spawn(symbolFC.prfab);
          go.transform.SetParent(_tr);
          go.transform.localScale = Vector3.one;
          go.transform.localPosition = SymbolPos[3] + new Vector3(0, yOffset, 0);
          symbolFc = go;
          //Debug.Log ("SymbolFC Created Pos:"+SymbolPos[3]);
        } else
        {
          Debug.Log("No Wild-FullColumn found!!!");
        }
      }

      if ((symbolFc != null) && (SpinCount == game.RowCount))
      {
        BenObjectPool.Unspawn(symbolFc);
        symbolFc = null;
      }

      for (int y = 0; y < game.RowCount + 1; ++y)
      {

        if (y == 0)
        {
          BenObjectPool.Unspawn(Symbols[0]);
          //Destroy(Symbols[0]);
        }

        if (y == game.RowCount)
        {
          Symbols[y] = null;
          SetSymbol(y, addedValue);
        } else
        {
          Symbols[y] = Symbols[y + 1];
        }

        Symbols[y].transform.localPosition = SymbolPos[y];
        Symbols[y].name = "Symbol" + y.ToString();
      }

      if (symbolFc != null)
      {
        symbolFc.transform.SetAsLastSibling();
      }
    }

    public void Spin()
    {
      _speed = 0.0f;
      MinimumRotateDistance = game.MinimumRotateDistance;
      bSlotIndicateFirst = false;
      StopOffset = -2;
      SpinCount = 0;
      _bWildFcExist = false;

      Vector3 vPos = _tr.localPosition;
      vPos.y = 0;
      _tr.localPosition = vPos;

      SetSymbol(game.RowCount, GetRandom());

      _inSpin = true;
      _inStop = false;
      InDamping = false;
    }

    public void ApplyResult (int [] values)
    {
      FinalValues = values;

      // Check if Wild Full Column is exist
      _bWildFcExist = false;

      for (int i = 0; i < game.RowCount; ++i)
      {
        if (game.GetSymbol(FinalValues[i]).type == SymbolType.WildFC)
        {
          _bWildFcExist = true;
          break;
        }
      }

      if (_bWildFcExist)
      {
        for (int i = 0; i < game.RowCount; ++i)
        {
          if (game.GetSymbol(FinalValues[i]).type == SymbolType.WildFC)
          {
            FinalValues[i] = game.GetSymbolIdxByType(SymbolType.Wild);
          }
        }
      }

    }

    public void Stop()
    {
      _inStop = true;
    }

    public bool Completed()
    {
      return (!_inStop && !_inSpin) ? true : false;
    }

  }

}