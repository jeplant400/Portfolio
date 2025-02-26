import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * Title displayed on the title screen.
 *
 */
public class Title extends Actor {

    private GreenfootImage normalImage;
    private GreenfootImage hoverImage;
    private int normalWidth;
    private int normalHeight;
    private int hoverWidth;
    private int hoverHeight;

    /**
     * Constructor for the Title class.
     */
    public Title() {
        normalImage = new GreenfootImage("title.png");
        hoverImage = new GreenfootImage("title hover.png");

        normalWidth = normalImage.getWidth();
        normalHeight = normalImage.getHeight();

        hoverWidth = (int) (normalWidth * 4.5);
        hoverHeight = (int) (normalHeight * 4.5);

        setImage(normalImage);
        scaleImage(normalImage, 4);
        scaleImage(hoverImage, hoverWidth, hoverHeight);
    }//end constructor Title

    /**
     * keeps image from freaking out
     */
    public void act() {
        handleMouseHover();
        handleMouseClick();
    }// end act

    /**
     * changes image on hover.
     */
    private void handleMouseHover() {
        if (Greenfoot.mouseMoved(this)) {
            setImage(hoverImage);
        } else if (Greenfoot.mouseMoved(null)) {
            setImage(normalImage);
        }
    }//end handleMouseHover

    /**
     * checks for mouse click
     */
    private void handleMouseClick() {
        if (Greenfoot.mouseClicked(this)) {
            Greenfoot.setWorld(new GameBoard());
        }
    }//end handleMouseClick

    /**
     * Scales a given image.
     *
     */
    private void scaleImage(GreenfootImage image, int scaleFactor) {
        if (image != null) {
            int newWidth = image.getWidth() * scaleFactor;
            int newHeight = image.getHeight() * scaleFactor;
            image.scale(newWidth, newHeight);
        }
    }// end scaleImage

    /**
     * another scale because of weird error
     *
     */
    private void scaleImage(GreenfootImage image, int newWidth, int newHeight) {
        if (image != null) {
            image.scale(newWidth, newHeight);
        }
    }//end scaleImage

    /**
     * Getter for the normal image.
     *
     */
    public GreenfootImage getNormalImage() {
        return normalImage;
    }//end getNormalImage

    /**
     * Getter for the hover image.
     *
     */
    public GreenfootImage getHoverImage() {
        return hoverImage;
    }//end getHoverImage
}
