# ALX-MVP-Project

MVP Idea: Sales Planet / A sales inventory Management Portal
======================================

A web application that will allow a user to purchase all varieties of products with ease.

### Notes
- Upload form
    - Upload photo
- Gallery
    - Thumbnail grid view.
    - On upload, a new entry is created with a default image to indicate work in progress.
    - When job is finished, thumbnail updates to show new post.
- Detailed single image view
    - Larger representation of image
    - Favs?

### Routes

- `/`: Index, gallery
    - `GET`: Display grid view of deep dream photos.
- `/api/images/<int:imageid>`
    - `GET`: Send image metadata.
    - `DELETE`: Delete the image with this id.
- `/api/images`
    - `GET`: Return a list of all images.
    - `POST`: Post a new image and send to deepdream API.
        - This should return 201 when the job is created.
        - Poll deepdream API. When job is done, retrieve image and update thumbnail/image data.

### Schema

- `image`
    - `metadata`
        - `id`
        - `timestamps`
    - `data`


