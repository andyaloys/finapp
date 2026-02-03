import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class YearService {
  private readonly STORAGE_KEY = 'selectedYear';
  private readonly DEFAULT_YEAR = new Date().getFullYear();
  
  private selectedYearSubject: BehaviorSubject<number>;
  public selectedYear$: Observable<number>;

  constructor() {
    const storedYear = this.getStoredYear();
    this.selectedYearSubject = new BehaviorSubject<number>(storedYear);
    this.selectedYear$ = this.selectedYearSubject.asObservable();
  }

  /**
   * Get currently selected year
   */
  getSelectedYear(): number {
    return this.selectedYearSubject.value;
  }

  /**
   * Set selected year
   */
  setSelectedYear(year: number): void {
    localStorage.setItem(this.STORAGE_KEY, year.toString());
    this.selectedYearSubject.next(year);
  }

  /**
   * Get available years (current year and 4 years back)
   */
  getAvailableYears(): number[] {
    const currentYear = new Date().getFullYear();
    const years: number[] = [];
    for (let i = 0; i <= 4; i++) {
      years.push(currentYear - i);
    }
    return years;
  }

  /**
   * Get year from localStorage or default
   */
  private getStoredYear(): number {
    const stored = localStorage.getItem(this.STORAGE_KEY);
    if (stored) {
      const year = parseInt(stored, 10);
      if (!isNaN(year)) {
        return year;
      }
    }
    return this.DEFAULT_YEAR;
  }

  /**
   * Reset to default year
   */
  resetToDefault(): void {
    this.setSelectedYear(this.DEFAULT_YEAR);
  }
}
