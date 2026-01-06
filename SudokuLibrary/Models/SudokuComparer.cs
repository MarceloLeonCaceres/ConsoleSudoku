namespace SudokuLibrary.Models;

public static class SudokuComparer
{
    // The logic you had in both classes, now in one place
    public static int CompareBoards(byte[,]? boardA, byte[,]? boardB, int knownA, int knownB)
    {
        if (knownA < knownB) return -1;
        if (knownA > knownB) return 1;

        if (boardA == null || boardB == null) return 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boardA[i, j] < boardB[i, j]) return -1;
                if (boardA[i, j] > boardB[i, j]) return 1;
            }
        }
        return 0;
    }

    public static bool AreBoardsEqual(byte[,]? boardA, byte[,]? boardB)
    {
        if (boardA == null && boardB == null) return true;
        if (boardA == null || boardB == null) return false;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boardA[i, j] != boardB[i, j]) return false;
            }
        }
        return true;
    }
}
