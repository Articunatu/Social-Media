return(
    <div>
        <h2>Enter your email.</h2>
        <div class="row">
            <div class="col-md-4">
                <form method="post">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                    <div class="form-floating">
                        <input asp-for="Input.Email" class="form-control" autocomplete="username" aria-required="true" />
                        <label asp-for="Input.Email" class="form-label"></label>
                        <span asp-validation-for="Input.Email" class="text-danger"></span>
                    </div>
                    <button type="submit" class="btn btn-dark">Reset Password</button>
                </form>
            </div>
        </div>
    </div>
)