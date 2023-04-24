$(document).ready(function () {
    $("#formEditMouseContainer").ready(function () {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Form edit mouse
        let form = $("#formEditMouse")

        addEventAutoPatternAntiSpecialCharacters(form.find(".inputName"))
        addEventAutoPatternAntiSpecialCharacters(form.find(".inputPhotoAlt"))
        addEventAutoPatternAntiSpecialCharacters(form.find(".inputComment"))

        mapCheckboxToInput(form.find(".checkboxIsWireless"), form.find(".inputIsWireless"))

        mapFileInputToInput(form.find(".inputPhoto"), form.find(".inputPhotoAsString"))

        form.find("#btnDeleteOldPhoto").click(function () {
            form.find("#btnDeleteOldPhoto").hide()
            form.find("#btnCancelDeleteOldPhoto").show()
            form.find("#currentMousePhoto").hide()
            form.find("#alertOldPhotoWillBeDeleted").show()
            form.find(".inputDeleteOldPhoto").val("true")
        })

        form.find("#btnCancelDeleteOldPhoto").click(function () {
            form.find("#btnDeleteOldPhoto").show()
            form.find("#btnCancelDeleteOldPhoto").hide()
            form.find("#currentMousePhoto").show()
            form.find("#alertOldPhotoWillBeDeleted").hide()
            form.find(".inputDeleteOldPhoto").val("false")
        })

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