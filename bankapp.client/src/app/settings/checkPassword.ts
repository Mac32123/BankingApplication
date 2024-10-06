import { AbstractControl, ValidationErrors } from "@angular/forms";

export function checkPasswords(group: AbstractControl): ValidationErrors | null {
  let parent = group.parent;
  let pass = parent?.get("newPassword")?.value;
  let confirmPass = parent?.get("confirmPassword")?.value
  return (pass === confirmPass && pass !== undefined) ? null : { notSame: true }
}
