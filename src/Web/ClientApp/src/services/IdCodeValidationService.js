export class IdCodeValidationService {

  isIdCodeValid(idCode: string): boolean {
    const isNumber = /^\d+$/.test(idCode);
    if (!isNumber) return false;

    if (idCode.length !== 11) return false;

    if (idCode.startsWith("0")) return false;

    const yearString = idCode.substring(1, 3);
    const monthString = idCode.substring(3, 5);
    const dayString = idCode.substring(5, 7);

    if (monthString === "00" || dayString === "00") return false;

    if (yearString !== "00" && !this.validateSectionOfIdCode(yearString, 0, 99)) return false;
    if (!this.validateSectionOfIdCode(monthString, 1, 12)) return false;
    if (!this.validateSectionOfIdCode(dayString, 1, 31)) return false;

    return true;
  }

  validateSectionOfIdCode(section: string, minValue: number, maxValue: number): boolean {
    const number = parseInt(section, 10);

    return number >= minValue && number <= maxValue;
  }
}