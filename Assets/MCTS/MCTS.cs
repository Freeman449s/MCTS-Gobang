using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTS : MonoBehaviour {
    Node root;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public MCTS(LevelType rootType) {
        root = new Node(rootType, null);
    }

    public Node selection() {
        // TODO
        return null;
    }
}
