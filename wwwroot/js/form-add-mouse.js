$(document).ready(function () {
    $("#formAddMouseContainer").ready(function () {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Form add mouse
        let form = $("#formAddMouse")

        addEventAutoPatternAntiSpecialCharacters(form.find(".inputName"))
        addEventAutoPatternAntiSpecialCharacters(form.find(".inputPhotoAlt"))

        mapCheckboxToInput(form.find(".checkboxIsWireless"), form.find(".inputIsWireless"))

        mapFileInputToInput(form.find(".inputPhoto"), form.find(".inputPhotoAsString"))

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Displaying toasts
        // toast form mouse (add mouse response)
        $("#toastResponseFormMouse").ready(function () {
            $("#toastResponseFormMouse").toast("show")
        })

        // toast form mouse (add mouse photo response)
        $("#toastResponseFormMouseAddPhoto").ready(function () {
            $("#toastResponseFormMouseAddPhoto").toast("show")
        })
    })
})