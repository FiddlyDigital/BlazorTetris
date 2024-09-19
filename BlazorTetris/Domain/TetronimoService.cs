namespace BlazorTetris.Domain
{
    public class TetronimoService
    {
        public Tetronimo CurrentTetronimo = null;
        public int Score = 0;
        private readonly Arena arena;
        private readonly Random random;

        private const string pieces = "ILJOTSZ";

        public TetronimoService(Arena arena)
        {
            this.arena = arena;
            random = new Random();
        }

        /// <summary>
        /// Drop the piece one row down in the arena, and see if it collides.
        /// If so, merge with the arena and calculate score
        /// </summary>
        public void DropOneRow()
        {
            this.CurrentTetronimo.Y++;

            if (this.arena.CollidesWith(this.CurrentTetronimo))
            {
                this.CurrentTetronimo.Y--; // can't place them, move them back up
                this.arena.Merge(this.CurrentTetronimo);
                this.Score += this.arena.SweepAndCalculateScore();
                this.Reset();
            }
        }

        /// <summary>
        /// Move the Tetronimo horizontally. If it collides, undo.
        /// </summary>
        /// <param name="dir">either Left (-1) or Right (+1)</param>
        public void Move(int dir)
        {
            this.CurrentTetronimo.X += dir;

            if (this.arena.CollidesWith(this.CurrentTetronimo))
            {
                this.CurrentTetronimo.X -= dir;
            }
        }

        public void Reset()
        {
            int[][] newShapeMatrix = TetronimoService.GetTetronimoMatrix(GetRandomShape());
            this.CurrentTetronimo = new Tetronimo()
            {
                Matrix = newShapeMatrix,
                Y = 0,
                X = (this.arena.Width / 2 | 0) - (newShapeMatrix.Length / 2 | 0)
            };

            // game over condition - can't place any more blocks
            if (this.arena.CollidesWith(this.CurrentTetronimo))
            {
                this.arena.Reset();
                this.Score = 0;
            }
        }

        public void Rotate(int dir)
        {
            int xPos = this.CurrentTetronimo.X;
            int offset = 1;
            this.CurrentTetronimo.Rotate(dir);

            // shunt a piece to the side if it collides after rotating
            while (this.arena.CollidesWith(this.CurrentTetronimo))
            {
                this.CurrentTetronimo.X += offset;
                offset = -(offset + (offset > 0 ? 1 : -1));

                if (offset > this.CurrentTetronimo.Matrix[0].Length)
                {
                    this.CurrentTetronimo.Rotate(-dir);
                    this.CurrentTetronimo.X = xPos;
                }
            }
        }

        private char GetRandomShape()
        {
            return pieces[random.Next(pieces.Length)];
        }

        private static int[][] GetTetronimoMatrix(char tetronimoShape)
        {
            return tetronimoShape switch
            {
                'T' => [
                        [0, 0, 0],
                        [1, 1, 1],
                        [0, 1, 0]
                    ],
                'O' => [
                        [2, 2],
                        [2, 2]
                    ],
                'L' => [
                        [0, 3, 0],
                        [0, 3, 0],
                        [0, 3, 3]
                    ],
                'J' => [
                        [0, 4, 0],
                        [0, 4, 0],
                        [4, 4, 0]
                    ],
                'I' => [
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0],
                        [0, 5, 0, 0]
                    ],
                'S' => [
                        [0, 6, 6],
                        [6, 6, 0],
                        [0, 0, 0]
                    ],
                'Z' => [
                        [7, 7, 0],
                        [0, 7, 7],
                        [0, 0, 0]
                    ],
                _ => throw new Exception("Invalid Piece Shape"),
            };
        }
    }
}
