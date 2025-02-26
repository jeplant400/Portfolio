import greenfoot.*;  // Importing Greenfoot classes

/**
 *  the computer's move in the game.
 *
 *
 */
public class ComputerMove extends Actor {

    private GreenfootImage moveImage;

    /**
     * Sets the image for the computer's move.
     *
     * 
     */
    public void setMoveImage(String move) {
        GreenfootImage image;

        if (move.equals("Rock")) {
            image = new GreenfootImage("rock.png");
        } else if (move.equals("Paper")) {
            image = new GreenfootImage("paper.png");
        } else {
            image = new GreenfootImage("scissors.png");
        }

        this.moveImage = image;
        setImage(moveImage);
        scaleImage(3.0); // Scale the image 
    }//end setMoveImage

    /**
     * Scales the image of the computer's move.
     *
     * 
     */
    private void scaleImage(double scaleFactor) {
        if (moveImage != null) {
            int newWidth = (int) (moveImage.getWidth() * scaleFactor);
            int newHeight = (int) (moveImage.getHeight() * scaleFactor);

            GreenfootImage scaledImage = new GreenfootImage(moveImage);
            scaledImage.scale(newWidth, newHeight);
            setImage(scaledImage);
        }
    }//end scaleImage

    /**
     * Getter for the current move image.
     *
     * 
     */
    public GreenfootImage getMoveImage() {
        return moveImage;
    }// end getMoveImage

    /**
     * Sets a new move image directly 
     *
     * 
     */
    public void setMoveImage(GreenfootImage moveImage) {
        this.moveImage = moveImage;
        setImage(moveImage);
    }//end setMoveImage
}
