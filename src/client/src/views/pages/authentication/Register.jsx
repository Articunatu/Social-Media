export default function Register() {
    return(
      <div className="register-wrapper">
        <h1>Enter info about you</h1>
        <form>
            <label>
                <p>Fullname</p>
                <input type="text" />
            </label>
            <label>
                <p>Username</p>
                <input type="text" />
            </label>
            <label>
                <p>Choose a password</p>
                <input type="password" />
            </label>
            <label>
                <p>Confirm password</p>
                <input type="password" />
            </label>
            <div>
                <button type="submit">Submit</button>
            </div>
        </form>
      </div>
    )
  }