using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Youkan2SelectPieceAlgorithm : SelectPieceAlgorithm
{
    public override PieceId SelectPiece(Piece[] state)
    {
        YoukanGameState nowState = new YoukanGameState(state);
        PieceId result = nowState.getBestPiece();
        return result;
    }

    // 汎用的なquartoAIクラス
    public class YoukanGameState
    {
        public Piece[] board;

        public YoukanGameState(Piece[] board)
        {
            this.board = board;
        }

        // 全てのコマの配列
        public List<PieceId> getAllPieces()
        {
            List<PieceId> allPieces = new List<PieceId>();
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null)
                {
                    allPieces.Add(board[i].getPieceId());
                }
            }
            return allPieces;
        }

        // 現在のボードの状況から渡すことが可能な配列（死ぬ可能性あり）
        public List<PieceId> getAvailablePieces()
        {
            // 全てのコマIDを取得
            List<PieceId> allPieceIds = Enum.GetValues(typeof(PieceId)) as PieceId[] != null
                ? new List<PieceId>((PieceId[])Enum.GetValues(typeof(PieceId)))
                : new List<PieceId>();
            // すでに配置されているコマIDを取得
            List<PieceId> placedPieceIds = getAllPieces();
            // 配置されていないコマIDのみ返す
            List<PieceId> availablePieces = new List<PieceId>();
            foreach (PieceId pieceId in allPieceIds)
            {
                if (!placedPieceIds.Contains(pieceId))
                {
                    availablePieces.Add(pieceId);
                }
            }
            return availablePieces;
        }

        // piece配列から安全なコマの配列を返す関数
        public List<PieceId> getSafePieces(PieceId[] pieceIds)
        {
            List<PieceId> safePieces = new List<PieceId>();
            foreach (PieceId pieceId in pieceIds)
            {
                if (this.isSafePiece(pieceId))
                {
                    safePieces.Add(pieceId);
                }
            }
            // 1つもない場合はランダムに1つ
            if (safePieces.Count == 0 && pieceIds.Length > 0)
            {
                // 修正: List<PieceId> から PieceId[] への変換
                int randIndex = UnityEngine.Random.Range(0, pieceIds.Length);
                return new List<PieceId>() { pieceIds[randIndex] };
            }
            return safePieces;
        }

        // PieceIdから渡した瞬間ゲームが終わるものを省いて即死しないPieceを返す関数
        public bool isSafePiece(PieceId pieceId)
        {
            // 今のボードをコピーして、駒を1つ進める
            // その状態で相手が勝つかどうかを判定
            // 仮実装: 常にtrue
            return true;
        }

        // ゲームの初手かどうかを判定
        public bool isFirstTurn()
        {
            return board.Count(p => p != null) == 0;
        }

        public PieceId getBestPiece()
        {
            List<PieceId> availablePieces = getAvailablePieces();

            // 1つもない場合はエラー
            if (availablePieces.Count == 0)
            {
                throw new Exception("渡すことが可能な駒がありません");
            }

            // availablePiecesからランダムに1つ
            PieceId randomPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Count)];
            // TODO - ここで渡してはいけないコマを渡さないようにバリデーションする
            return randomPiece;
        }

        public Position getPutPosition(PieceId pieceId){
            // おける場所を探す、ランダムに選んだpositionを返す
            List<Position> puttablePositions = getPuttablePositions(pieceId);
            return puttablePositions[UnityEngine.Random.Range(0, puttablePositions.Count)];
        }

        public List<Position> getPuttablePositions(PieceId pieceId){
            // おける場所を探す
            List<Position> puttablePositions = new List<Position>();
            for(int i = 0; i < board.Length; i++){
                if(board[i] == null){
                    puttablePositions.Add(new Position(i % 4, i / 4));
                }
            }
            return puttablePositions;
        }
    }
}