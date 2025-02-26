import greenfoot.*;  // Importing Greenfoot classes

/**
 * RPS AI
 *
 */
public class Computer {

    private String lastMove;

    /**
     * Generates a random move for the computer.
     *
     */
    public String generateMove() {
        String[] moves = {"Rock", "Paper", "Scissors"};
        int randomIndex = Greenfoot.getRandomNumber(moves.length);
        lastMove = moves[randomIndex];
        return lastMove;
    }//end generateMove

    /**
     * Getter for the last move made by the computer.
     *
     */
    public String getLastMove() {
        return lastMove;
    }// getLastMove

    /**
     * Sets the last move for the computer.
     *
     */
    public void setLastMove(String move) {
        this.lastMove = move;
    }// end setLastMove

    /**
     * Determines the winner.
     *
     */
    public String determineWinner(String playerMove) {
        if (playerMove.equals(lastMove)) {
            return "Draw! Both chose " + playerMove;
        } else if ((playerMove.equals("Rock") && lastMove.equals("Scissors")) ||
                   (playerMove.equals("Paper") && lastMove.equals("Rock")) ||
                   (playerMove.equals("Scissors") && lastMove.equals("Paper"))) {
            return "You Win! " + playerMove + " beats " + lastMove;
        } else {
            return "You Lose! " + lastMove + " beats " + playerMove;
        }
    }//end determineWinner
}
