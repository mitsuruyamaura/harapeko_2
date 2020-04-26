using UnityEngine;

[System.Serializable]
public struct RangeInteger {
    public RangeInteger(int i_value) {
        m_minValue = i_value;
        m_maxValue = i_value;
    }

    public RangeInteger(int i_min, int i_max) {
        m_minValue = i_min;
        m_maxValue = Mathf.Max(i_min, i_max);    //  小さい方を基準にする
    }

    [SerializeField] private int m_minValue;
    [SerializeField] private int m_maxValue;

    public int MinValue {
        get { return m_minValue; }
        set { m_minValue = Mathf.Min(value, m_maxValue); }
    }

    public int MaxValue {
        get { return m_maxValue; }
        set { m_maxValue = Mathf.Max(value, m_minValue); }
    }
}
