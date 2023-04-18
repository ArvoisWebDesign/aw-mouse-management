$(document).ready(function () {
    $("#formEditMouseContainer").ready(function () {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Form edit mouse
        let form = $("#formEditMouse")

        addEventAutoPatternAntiSpecialCharacters(form.find(".inputName"))
        addEventAutoPatternAntiSpecialCharacters(form.find(".inputPhotoAlt"))

        mapCheckboxToInput(form.find(".checkboxIsWireless"), form.find(".inputIsWireless"))

        mapFileInputToInput(form.find(".inputPhoto"), form.find(".inputPhotoAsString"))

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Displaying toasts
        // toast form mouse (edit mouse response)
        $("#toastResponseFormMouse").ready(function () {
            $("#toastResponseFormMouse").toast("show")
        })

        // toast form mouse (add mouse photo response)
        $("#toastResponseFormMouseAddPhoto").ready(function () {
            $("#toastResponseFormMouseAddPhoto").toast("show")
        })

        // toast form mouse (delete mouse photo response)
        $("#toastResponseFormMouseDeletePhoto").ready(function () {
            $("#toastResponseFormMouseDeletePhoto").toast("show")
        })
    })
})