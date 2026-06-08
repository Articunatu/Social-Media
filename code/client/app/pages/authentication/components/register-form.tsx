import Link from "next/link";
import { SignUpField } from "./sign-up-field";
import { useRegisterForm } from "../hooks/use-register-form";
import { signUpLimits } from "../validation-utils/sign-up-validation";

export const RegisterForm: React.FC = () => {
    const { auth, canSubmit, errors, handleChange, handleSubmit, localError, values } = useRegisterForm();
    const error = localError || auth?.error;

    return (
        <form onSubmit={handleSubmit} className="card bg-base-100 border border-base-300 shadow-sm" noValidate>
            <div className="card-body">
                <div className="mb-2">
                    <h2 className="card-title text-2xl">Sign up</h2>
                    <p className="text-sm text-base-content/70">
                        Already have an account?{" "}
                        <Link href="/" className="link link-primary font-semibold">
                            Log in
                        </Link>
                    </p>
                </div>

                <div className="grid gap-4 sm:grid-cols-2">
                    <SignUpField
                        id="firstName"
                        label="First name"
                        value={values.firstName}
                        onChange={handleChange("firstName")}
                        error={errors.firstName}
                        maxLength={signUpLimits.firstNameMaxLength}
                        autoComplete="given-name"
                    />
                    <SignUpField
                        id="lastName"
                        label="Last name"
                        value={values.lastName}
                        onChange={handleChange("lastName")}
                        error={errors.lastName}
                        maxLength={signUpLimits.lastNameMaxLength}
                        autoComplete="family-name"
                    />
                </div>

                <div className="grid gap-4">
                    <SignUpField
                        id="tag"
                        label="Tag"
                        value={values.tag}
                        onChange={handleChange("tag")}
                        error={errors.tag}
                        maxLength={signUpLimits.tagMaxLength}
                        autoComplete="username"
                    />
                    <SignUpField
                        id="email"
                        label="Email"
                        type="email"
                        value={values.email}
                        onChange={handleChange("email")}
                        error={errors.email}
                        maxLength={signUpLimits.emailMaxLength}
                        autoComplete="email"
                    />
                    <SignUpField
                        id="password"
                        label="Password"
                        type="password"
                        value={values.password}
                        onChange={handleChange("password")}
                        error={errors.password}
                        maxLength={signUpLimits.passwordMaxLength}
                        autoComplete="new-password"
                    />
                    <SignUpField
                        id="confirmPassword"
                        label="Confirm password"
                        type="password"
                        value={values.confirmPassword}
                        onChange={handleChange("confirmPassword")}
                        error={errors.confirmPassword}
                        maxLength={signUpLimits.passwordMaxLength}
                        autoComplete="new-password"
                    />
                </div>

                {error && <div className="alert alert-error mt-2 text-sm">{error}</div>}

                <button type="submit" className="btn btn-primary mt-2 w-full" disabled={!canSubmit}>
                    {auth?.loading ? "Creating account..." : "Create account"}
                </button>
            </div>
        </form>
    );
};
