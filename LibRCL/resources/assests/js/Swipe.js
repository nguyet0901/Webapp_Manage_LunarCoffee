class Swipe {
    constructor(element) {
        this.xDown = null;
        this.yDown = null;
        this.element = typeof (element) === 'string' ? document.querySelector(element) : element;

        if (this.element != null) {
            this.element.addEventListener('touchstart', function (evt) {
                this.xDown = evt.touches[0].clientX;
                this.yDown = evt.touches[0].clientY;
            }.bind(this), false);
        }
    }

    onLeft(callback) {
        this.onLeft = callback;

        return this;
    }

    onRight(callback) {
        this.onRight = callback;

        return this;
    }

    onUp(callback) {
        this.onUp = callback;

        return this;
    }

    onDown(callback) {
        this.onDown = callback;

        return this;
    }

    handleTouchMove(evt) {
        if (!this.xDown || !this.yDown) {
            return;
        }

        var xUp = evt.touches[0].clientX;
        var yUp = evt.touches[0].clientY;

        this.xDiff = this.xDown - xUp;
        this.yDiff = this.yDown - yUp;

        if (Math.abs(this.xDiff) > Math.abs(this.yDiff)) { // Most significant.
            if (this.xDiff < -30) {
                if (this.onLeft != "undefined" && (typeof this.onLeft != undefined)) this.onLeft();

            } else if (this.xDiff > 30) {
                if (this.onRight != "undefined" && (typeof this.onRight != undefined)) this.onRight();
            }
        } else {
            if (this.yDiff > 30) {
                if (this.onUp != "undefined" && (typeof this.onUp != undefined)) this.onUp();
            } else if (this.yDiff < -30) {
                if (this.onDown != "undefined" && (typeof this.onDown != undefined)) this.onDown();
            }
        }

        // Reset values.
        this.xDown = null;
        this.yDown = null;
    }

    destroy() {
        if (this.element != null) {
            this.element.removeEventListener('touchmove');
            this.element.removeEventListener('touchstart');
        }
    }

    run() {
        if (this.element != null) {
            this.element.addEventListener('touchmove', function (evt) {
                this.handleTouchMove(evt);
            }.bind(this), false);
        }
        
    }
}