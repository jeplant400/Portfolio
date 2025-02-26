import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * where RPS happens
 *
 */
public class Tabletop extends Actor {

    private Text resultText;
    private int playerScore;
    private int computerScore;
    private GreenfootImage scoreBoard;
    private boolean tabletopVisible;

    /**
     * Constructor for the Tabletop class.
     */
    public Tabletop() {
        scaleImage();
        this.playerScore = 0;
        this.computerScore = 0;
        this.tabletopVisible = true;
    }//ende constructor Tabletop

    /**
     * Scales the image of the tabletop.
     */
    private void scaleImage() {
        GreenfootImage myImage = getImage();
        if (myImage != null) {
            int myNewHeight = myImage.getHeight() * 15;
            int myNewWidth = myImage.getWidth() * 15;
            myImage.scale(myNewWidth, myNewHeight);
        }
    }//end scaleImage

    /**
     * Removes the tabletop from the game world.
     */
    public void removeTabletop() {
        getWorld().removeObject(this);
        this.tabletopVisible = false;
        System.out.println("REMOVING TABLETOP");
    }//end removeTabletop

    /**
     * Getter for the player score.
     *
     */
    public int getPlayerScore() {
        return playerScore;
    }//end getPlayerScore

    /**
     * Setter for the player score.
     *
     */
    public void setPlayerScore(int playerScore) {
        this.playerScore = playerScore;
        updateScoreBoard();
    }//end setPlayerScore

    /**
     * Getter for the computer score.
     *
     */
    public int getComputerScore() {
        return computerScore;
    }// end getComputerScore

    /**
     * Setter for the computer score.
     *
     */
    public void setComputerScore(int computerScore) {
        this.computerScore = computerScore;
        updateScoreBoard();
    }// end setComputerScore

    /**
     * Updates the score.
     */
    private void updateScoreBoard() {
        if (scoreBoard == null) {
            scoreBoard = new GreenfootImage(200, 50);
            scoreBoard.setColor(Color.BLACK);
            scoreBoard.setFont(new Font("Arial", 24));
        }

        scoreBoard.clear();
        scoreBoard.drawString("Player: " + playerScore + "  Computer: " + computerScore, 10, 30);
        getWorld().showText("Player: " + playerScore, 50, 50);
        getWorld().showText("Computer: " + computerScore, 500, 50);
    }//end updateScoreBoard

    /**
     * Checks if the tabletop is visible.
     *
     */
    public boolean isTabletopVisible() {
        return tabletopVisible;
    }// end isTabletopVisible
}
