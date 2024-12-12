import { Pipe, PipeTransform } from '@angular/core';
import { Bike } from '../home/home.component';

@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {

  transform(bikes: Bike[], searchBrand: string, searchRent: number | null): Bike[] { // Change return type to Bike[]
    if (!bikes) return bikes; // Return bikes directly if null/undefined

    let filteredBikes = bikes;

    // Filter by brand
    if (searchBrand) {
      const lowerSearchBrand = searchBrand.toLowerCase();
      filteredBikes = filteredBikes.filter((bike) =>
        bike.brandName.toLowerCase().includes(lowerSearchBrand)
      );
    }

    // Filter by rent
    if (searchRent != null) {
      filteredBikes = filteredBikes.filter((bike) =>
        bike.bikeUnits.some((unit) => unit.rentPerDay <= searchRent)
      );
    }

    return filteredBikes; // Correctly return Bike[]
  }
}
