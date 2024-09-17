using System.Drawing;

namespace BlazorTetris.Domain
{
    // TODO: Split Player from Tetronimo
    public class Player
    {
        Random random;
        public int[][] matrix = null;
        private Arena arena;
        public int score = 0;
        public Point pos;

        private const string pieces = "ILJOTSZ";

        public Player(Arena arena)
        {
            this.arena = arena;
            this.pos = new Point(0, 0);
            random = new Random();
        }

        private int[][] createPiece(char shape)
        {
            switch (shape)
            {
                case 'T':
                    return [
                        [0, 0, 0],
                        [1, 1, 1],
                        [0, 1, 0]
                    ];
                case 'O':
                    return [
                        [2, 2],
                        [2, 2]
                    ];
                case 'L':
                    return [
                        [0, 3, 0],
                        [0, 3, 0],
                        [0, 3, 3]
                    ];
                case 'J':
                    return [
                        [0, 4, 0],
                        [0, 4, 0],
                        [4, 4, 0]
                    ];
                case 'I':
                    return [
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0]
                    ];
                case 'S':
                    return [
                        [0, 6, 6],
                        [6, 6, 0],
                        [0, 0, 0]
                    ];
                case 'Z':
                    return [
                        [7, 7, 0],
                        [0, 7, 7],
                        [0, 0, 0]
                    ];
                default:
                    throw new Exception("Invalid Piece Shape");
            }
        }

        public void drop()
        {
            this.pos.Y++;

            if (this.arena.collides(this))
            {
                this.pos.Y--; // can't place them, move them back up
                this.arena.merge(this);
                this.score += this.arena.SweepAndScore();
                this.reset();
            }
        }

        private char GetRandomShape()
        {
            return pieces[random.Next(pieces.Length)];
        }

        public void move(int dir)
        {
            this.pos.X += dir;

            if (this.arena.collides(this))
            {
                this.pos.X -= dir;
            }
        }

        // Matrix rotation = Transpose + Reverse
        public void pieceRotate(int[][] matrix, int dir)
        {
            // Transpose Step
            for (int y = 0; y < matrix.Length; ++y)
            {
                for (int x = 0; x < y; ++x)
                {
                    int swapVal = matrix[x][y];
                    matrix[x][y] = matrix[y][x];
                    matrix[y][x] = swapVal;
                }
            }

            // Reverse Step
            if (dir > 0)
            {
                // if direciton is positive
                for (int y = 0; y < matrix.Length; y++)
                {
                    Array.Reverse(matrix[y]);
                }
            }
            else
            {
                // if direciton is negative
                Array.Reverse(matrix);
            }

        }

        public void reset()
        {
            this.matrix = this.createPiece(GetRandomShape());
            this.pos.Y = 0;
            this.pos.X = (this.arena.matrix[0].Length / 2 | 0) - (this.matrix.Length / 2 | 0);

            // game over condition - can't place any more blocks
            if (this.arena.collides(this))
            {
                this.arena.Reset();
                this.score = 0;
            }
        }

        public void rotate(int dir)
        {
            int xPos = this.pos.X;
            int offset = 1;
            this.pieceRotate(this.matrix, dir);

            // shunt a piece to the side if it collides after rotating
            while (this.arena.collides(this))
            {
                this.pos.X += offset;
                offset = -(offset + (offset > 0 ? 1 : -1));

                if (offset > this.matrix[0].Length)
                {
                    this.pieceRotate(this.matrix, -dir);
                    this.pos.X = xPos;
                }
            }
        }
    }
}
