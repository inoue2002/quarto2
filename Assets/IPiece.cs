using UnityEngine;

public interface IPiece
{
    void Select();
    void Deselect();
    bool IsSelected { get; }
}
