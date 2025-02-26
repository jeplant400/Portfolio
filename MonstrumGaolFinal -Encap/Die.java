import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * The Die 
 *
 * 
 */
public class Die extends Actor {

    private int dieValue;

    /**
     * Constructor for the Die class.
     * Scales the image of the die.
     */
    public Die() {
        scaleImage();
    }//end constructor Die

    /**
     * Updates the image of the die based on the current number.
     * 
     * 
     */
    public void updateImage(int dieValue) {
        
        if (dieValue >= 0 && dieValue <= 4) {
            this.dieValue = dieValue;
            setImage("die" + dieValue + ".png"); 
            scaleImage();
        } else {
            System.out.println("Invalid die value: " + dieValue);
        }
    }//end updateImage

    /**
     * Scales the current image 
     */
    private void scaleImage() {
        GreenfootImage myImage = getImage();
        if (myImage != null) {
            int myNewHeight = (int) myImage.getHeight() * 3;
            int myNewWidth = (int) myImage.getWidth() * 3;
            myImage.scale(myNewWidth, myNewHeight);
        }
    }//end scaleImage

    /**
     * Getter for the current die number.
     *
     * 
     */
    public int getDieValue() {
        return dieValue;
    }//end getDieValue

}
